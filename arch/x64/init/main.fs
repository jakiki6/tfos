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

LIT" [*] uefi systable: " klog uefi-systable klog-buf utils-printh klog-nl
LIT" [*] uefi oem: " klog uefi-systable s-uefi-systable/firmware-vendor klog-buf utils-print16 klog-nl

begin again

cpu-mem-init

cpu-paging-init

cpu-gdt-init
cpu-idt-init

base-init

cpu-sched-init
sched-init

dev-pit-init
dev-pic-init

fb-init

cpu-irq-enable

LIT" [*] booting tfos...\n" klog

LIT" [*] memory map:\n" klog
cpu-mem-e820-count >r begin
  cpu-mem-e820-count r@ - 24 * cpu-mem-e820-head +
    $20 tty-buf 2dup utils-printc utils-printc
    dup @ klog-buf utils-printh $20 klog-c 8 +
    dup @ klog-buf utils-printh $20 klog-c 8 +
    d@ klog-buf utils-printh klog-nl
next

acpi-init

val init-wake
val init-wake-addr

( the init can't die or we're screwed )
begin
  1 v' init-wake sched-waitfor
  init-wake-addr execute
again
