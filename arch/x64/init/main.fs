include kernel/base.fs

include arch/x64/cpu/gdt.fs
include arch/x64/cpu/idt.fs
include arch/x64/cpu/sched.fs
include arch/x64/dev/pic.fs
include arch/x64/dev/serial.fs

cpu-gdt-init
cpu-idt-init

cpu-sched-init
sched-init

dev-pic-init
serial-init

cpu-irq-enable

val fb
$b8000 to fb

: print begin
  dup c@ dup serial-out fb c! fb 1+ $1f c! fb 2 + to fb 1+ dup c@ not
until ;

LIT" booting tfos..." print

$f0000 $b8140 480 mem-copy
val ptr
$ffff5 to ptr
$b8078 to fb
8 begin ptr c@ fb c! ptr 1+ to ptr fb 2 + to fb 1- dup 0 = until drop

: foo1 begin $41 $b8068 c! again ;
: foo2 begin $42 $b8068 c! again ;

' arch-hlt sched-add

begin
  cookie-push
  $100
  begin $b8320
    begin dup rng swap c! 1+ dup $b8fa0 = until
  drop 1- dup not until drop

  $b8090 @ 1+ $b8090 !
  cookie-pop
again
