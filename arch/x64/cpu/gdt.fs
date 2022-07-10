val cpu-gdt-tbl
val cpu-gdt-desc

: cpu-gdt-init
  34 mem-alloc to cpu-gdt-tbl

  $0000000000000000 cpu-gdt-tbl 0 + !
  $00209A0000000000 cpu-gdt-tbl 8 + !
  $0000920000000000 cpu-gdt-tbl 16 + !
  23 cpu-gdt-tbl 24 + w!
  cpu-gdt-tbl dup 26 + !

  cpu-gdt-tbl 24 + to cpu-gdt-desc

  cpu-gdt-desc arch-lidt ;
