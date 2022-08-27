val boot-handover
to boot-handover

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

LIT" [*] booting tfos...\n" tty-buf buf-print

LIT" [*] memory map:\n" tty-buf buf-print
cpu-mem-e820-count >r begin
  cpu-mem-e820-count r@ - 24 * cpu-mem-e820-head +
    $20 tty-buf 2dup utils-printc utils-printc
    dup @ tty-buf utils-printh $20 tty-buf utils-printc 8 +
    dup @ tty-buf utils-printh $20 tty-buf utils-printc 8 +
    d@ tty-buf utils-printh $0a tty-buf utils-printc
next

acpi-init
LIT" [*] acpi rsdp table: " tty-buf buf-print acpi-rsdp tty-buf utils-printh
$0a tty-buf utils-printc

val init-wake
val init-wake-addr

( the init can't die or we're screwed )
begin
  1 v' init-wake sched-waitfor
  init-wake-addr execute
again
