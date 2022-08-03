val cpu-paging-pml4
val cpu-paging-pml3
val cpu-paging-pml2

: cpu-paging-init
  $1000 mem-alloc to cpu-paging-pml4
  $1000 mem-alloc to cpu-paging-pml3
  $1000 mem-alloc to cpu-paging-pml2

  cpu-paging-pml4 $1000 mem-wipe
  cpu-paging-pml3 $1000 mem-wipe
  cpu-paging-pml2 $1000 mem-wipe

  cpu-paging-pml3 $03 or cpu-paging-pml4 !
  cpu-paging-pml2 $03 or cpu-paging-pml3 !

  512 do
    i 21 << $83 or cpu-paging-pml2 i 3 << + !
  loop

  cpu-paging-pml4 arch-cr3
;
