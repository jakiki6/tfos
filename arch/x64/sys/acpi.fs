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
;
