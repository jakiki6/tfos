to uefi-systable
to uefi-handle

val boot-handover

include sys/base.fs

include arch/x64/cpu/gdt.fs
include arch/x64/cpu/idt.fs
include arch/x64/cpu/sched.fs
include arch/x64/cpu/mem.fs
include arch/x64/cpu/paging.fs
include arch/x64/sys/acpi.fs
include arch/x64/dev/pit.fs
include arch/x64/dev/pic.fs
include arch/x64/dev/serial.fs
include arch/x64/dev/vesa.fs

( early debugging )
dev-serial-init

uefi-init

LIT" [*] cpu: initializing internal state...\n" klog
cpu-paging-init
cpu-gdt-init
cpu-idt-init

LIT" [*] sys: initializing subsystems...\n" klog
base-init

cpu-sched-init
sched-init

dev-pit-init
dev-pic-init

cpu-irq-enable

LIT" [*] sys: done, booting tfos...\n" klog

val init-wake
val init-wake-addr

( the init can't die or we're screwed )
begin
  1 v' init-wake sched-waitfor
  init-wake-addr execute
again
