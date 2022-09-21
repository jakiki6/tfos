val uefi-systable
val uefi-handle

val uefi-srv-boot
val uefi-srv-runtime

val uefi-memmap-size
val uefi-memmap-ptr
val uefi-memmap-count
val uefi-memmap-key
val uefi-memmap-desc-size
val uefi-memmap-desc-ver

: uefi-init
  LIT" [*] uefi: systable is at " klog uefi-systable klog-buf utils-printh klog-nl
  LIT" [*] uefi: handle is " klog uefi-handle klog-buf utils-printh klog-nl
  LIT" [*] uefi: oem: " klog uefi-systable s-uefi-systable/firmware-vendor klog-buf utils-print16 klog-nl

  uefi-systable s-uefi-systable/boot-services to uefi-srv-boot
  uefi-systable s-uefi-systable/runtime-services to uefi-srv-runtime

  ( get memory map, allocate space )
  $10000 to uefi-memmap-size
  2 uefi-memmap-size v' uefi-memmap-ptr 0 0 0
    uefi-srv-boot s-uefi-srv-boot/allocate-pool uefi-call if
      LIT" uefi: cannot allocate buffer for memory map" panic
    then

  LIT" [*] uefi: memory map is at " klog uefi-memmap-ptr klog-buf utils-printh LIT"  size " klog uefi-memmap-size klog-buf utils-printh klog-nl

  v' uefi-memmap-size uefi-memmap-ptr v' uefi-memmap-key v' uefi-memmap-desc-size v' uefi-memmap-desc-ver 0
    uefi-srv-boot s-uefi-srv-boot/get-memory-map uefi-call if
      LIT" uefi: cannot obtain memory map" panic
    then

  uefi-memmap-size uefi-memmap-desc-size / to uefi-memmap-count
  LIT" [*] uefi: number of memory map entries: " klog uefi-memmap-count klog-buf utils-printh klog-nl

  uefi-handle uefi-memmap-key 0 0 0 0
    uefi-srv-boot s-uefi-srv-boot/exit-boot-srv uefi-call if
      LIT" uefi: cannot exit boot services" panic
    then

  LIT" [*] uefi: exited boot services\n" klog

  uefi-mem-init
;

val uefi-mem-tmp

: uefi-mem-init

  uefi-memmap-ptr
  uefi-memmap-count do
     dup s-uefi-memmap-desc/type @ 7 = if
       dup s-uefi-memmap-desc/phys @ 
       over s-uefi-memmap-desc/pages @ 12 <<
       +
       mem-mm-highest max to mem-mm-highest
     then

     uefi-memmap-desc-size +
  loop

  LIT" [*] mem: highest page: " klog mem-mm-highest klog-h klog-nl

  mem-mm-highest 12 >> 7 + 3 >> $fff + 12 >> to uefi-mem-tmp

  uefi-memmap-ptr
  uefi-memmap-count do
    dup s-uefi-memmap-desc/type @ 7 = if
      dup s-uefi-memmap-desc/pages uefi-mem-tmp > if
        dup s-uefi-memmap-desc/phys to mem-mm-ptr
	dup s-uefi-memmap-desc/pages @ uefi-mem-tmp - s-uefi-memmap-desc/pages !
	dup s-uefi-memmap-desc/phys @ uefi-mem-tmp + s-uefi-memmap-desc/phys !

	-1 to uefi-mem-tmp
      then
    then

    uefi-memmap-desc-size +
  loop

  mem-mm-ptr 0 = if
    LIT" mem: cannot find an area for the bitmap" panic
  then

  LIT" [*] mem: bitmap address: " klog mem-mm-ptr klog-h klog-nl
;
