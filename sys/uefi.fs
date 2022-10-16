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

val uefi-prot-gop
val uefi-prot-gop-num-modes

: uefi-init
  LIT" [*] uefi: systable is at " klog uefi-systable klog-h klog-nl
  LIT" [*] uefi: handle is " klog uefi-handle klog-h klog-nl
  LIT" [*] uefi: oem: " klog uefi-systable s-uefi-systable/firmware-vendor klog-buf utils-print16 klog-nl

  uefi-systable s-uefi-systable/boot-services to uefi-srv-boot
  uefi-systable s-uefi-systable/runtime-services to uefi-srv-runtime

  ( get memory map, allocate space )
  $10000 to uefi-memmap-size
  2 uefi-memmap-size v' uefi-memmap-ptr 0 0 0
    uefi-srv-boot s-uefi-srv-boot/allocate-pool uefi-call if
      LIT" uefi: cannot allocate buffer for memory map" panic
    then

  LIT" [*] uefi: memory map is at " klog uefi-memmap-ptr klog-h LIT"  size " klog uefi-memmap-size klog-h klog-nl

  v' uefi-memmap-size uefi-memmap-ptr v' uefi-memmap-key v' uefi-memmap-desc-size v' uefi-memmap-desc-ver 0
    uefi-srv-boot s-uefi-srv-boot/get-memory-map uefi-call if
      LIT" uefi: cannot obtain memory map" panic
    then

  uefi-memmap-size uefi-memmap-desc-size / to uefi-memmap-count
  LIT" [*] uefi: number of memory map entries: " klog uefi-memmap-count klog-h klog-nl
  LIT" [*] uefi: memory map descriptor size: " klog uefi-memmap-desc-size klog-h klog-nl

  ( locate GOP )
  c-uefi-guid-gop 0 v' uefi-prot-gop 0 0 0
    uefi-srv-boot s-uefi-srv-boot/locate-protocol uefi-call if
      LIT" uefi: cannot locate GOP" panic
    then

  LIT" [*] uefi: GOP is at " klog uefi-prot-gop klog-h klog-nl

  uefi-prot-gop s-uefi-prot-gop/mode s-uefi-prot-gop-mode/max-mode to uefi-prot-gop-num-modes
  LIT" [*] uefi: number of GOP modes: " klog uefi-prot-gop-num-modes klog-d klog-nl

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
     dup s-uefi-memmap-desc/type 7 = if
       dup s-uefi-memmap-desc/pages uefi-mem-tmp + to uefi-mem-tmp

       dup s-uefi-memmap-desc/phys 
       over s-uefi-memmap-desc/pages 12 <<
       +
       mem-mm-highest max to mem-mm-highest
     then

     uefi-memmap-desc-size +
  loop

  LIT" [*] mem: usable memory: " klog uefi-mem-tmp 8 >> klog-d LIT" MB\n" klog
  LIT" [*] mem: highest page: " klog mem-mm-highest klog-h klog-nl

  mem-mm-highest 12 >> 7 + 3 >> $fff + 12 >> to uefi-mem-tmp

  uefi-memmap-ptr
  uefi-memmap-count do
    dup s-uefi-memmap-desc/type 7 = if
      dup s-uefi-memmap-desc/pages uefi-mem-tmp > if
        dup s-uefi-memmap-desc/phys to mem-mm-ptr
	dup s-uefi-memmap-desc/pages uefi-mem-tmp - s-uefi-memmap-desc/pages !
	dup s-uefi-memmap-desc/phys uefi-mem-tmp + s-uefi-memmap-desc/phys ! 

	-1 to uefi-mem-tmp
      then
    then

    uefi-memmap-desc-size +
  loop

  mem-mm-ptr 0 = if
    LIT" mem: cannot find an area for the bitmap" panic
  then

  LIT" [*] mem: bitmap address: " klog mem-mm-ptr klog-h klog-nl

  mem-mm-ptr mem-mm-highest 12 >> 7 + 3 >> $fff + 12 >> mem-wipe

  uefi-memmap-ptr
  uefi-memmap-count do
    dup s-uefi-memmap-desc/type 7 = if
      dup s-uefi-memmap-desc/phys 12 >> 7 + 3 >> mem-mm-ptr +
      over s-uefi-memmap-desc/pages 3 >>
      $ff
      mem-set
    then

    uefi-memmap-desc-size +
  loop 

  LIT" [*] mem: populated bitmap" klog klog-nl
;
