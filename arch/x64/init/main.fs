val boot-handover
to boot-handover

include kernel/base.fs

include arch/x64/cpu/gdt.fs
include arch/x64/cpu/idt.fs
include arch/x64/cpu/sched.fs
include arch/x64/cpu/mem.fs
include arch/x64/cpu/paging.fs
include arch/x64/dev/pit.fs
include arch/x64/dev/pic.fs
include arch/x64/dev/serial.fs
include arch/x64/dev/vesa.fs

cpu-mem-init
cpu-paging-init

cpu-gdt-init
cpu-idt-init

base-init

cpu-sched-init
sched-init

dev-pit-init
dev-pic-init
serial-init

fb-init
( 255 255 255 69 69 fb-draw-one )

cpu-irq-enable

: print begin
  dup c@ serial-out 1+ dup c@ not
until ;

LIT" booting tfos..." print $0a serial-out

val x
val y

begin
  x y fb-get
  1+ rot> 1+ rot> 1+ rot>
  x y fb-put

  x rng-pm1 + fb-info-width % to x
  y rng-pm1 + fb-info-height % to y
again

arch-hlt
