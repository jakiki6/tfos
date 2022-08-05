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

cpu-irq-enable

: print begin
  dup c@ dup serial-out tty-write-one 1+ dup c@ not
until ;

LIT" booting tfos..." print $0a serial-out

val init-wake
val init-wake-addr

( the init can't die or we're screwed )
begin
  1 v' init-wake sched-waitfor
  init-wake-addr execute
again
