val cpu-paging-pml4
val cpu-paging-pml3
val cpu-paging-pml2s

: cpu-paging-init
  $1000 mem-alloc to cpu-paging-pml4
  $1000 mem-alloc to cpu-paging-pml3
  $4000 mem-alloc to cpu-paging-pml2s

  cpu-paging-pml4 $1000 mem-wipe
  cpu-paging-pml3 $1000 mem-wipe
  cpu-paging-pml2s $4000 mem-wipe

  cpu-paging-pml3 $03 or cpu-paging-pml4 !

  4 do 
    cpu-paging-pml2s i 12 << + $03 or cpu-paging-pml3 i 3 << + !
  loop

  2048 do
    i 21 << $83 or cpu-paging-pml2s i 3 << + !
  loop

  cpu-paging-pml4 arch-cr3

  ( now, that we have 4gb of ram mapped, we can map more )
  $200000 mem-alloc to cpu-paging-pml2s

  262144 do
    i 21 << $83 or cpu-paging-pml2s i 3 << + !
  loop

  512 do
    cpu-paging-pml2s i 12 << + $03 or cpu-paging-pml3 i 3 << + !
  loop

  ( invalidate tlb, technically unnecessary but idc )
  cpu-paging-pml4 arch-cr3
;
