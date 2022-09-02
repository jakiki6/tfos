val acpi-rsdp

: acpi-find-rsdp
  $ffff do
    i 4 << @ $2052545020445352 = if
      i 4 << to acpi-rsdp
    then
  loop

  acpi-rsdp 0 = if
    LIT" RSDP table not found" panic
  then
;

: acpi-init
  acpi-find-rsdp

  LIT" [*] acpi rsdp table: " klog acpi-rsdp klog-buf utils-printh
  klog-nl

  LIT" [*] acpi oem: " klog
  acpi-rsdp 9 + 6 klog-w
  klog-nl

  LIT" [*] acpi version: " klog
  acpi-rsdp 15 + c@ klog-buf utils-printn
  klog-nl
;
