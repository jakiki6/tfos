val uefi-systable
val uefi-handle

val uefi-srv-boot
val uefi-srv-runtime

val uefi-memmap-size
val uefi-memmap-ptr
val uefi-memmap-key
val uefi-memmap-desc-size
val uefi-memmap-desc-ver

: uefi-init
  LIT" [*] uefi: systable is at " klog uefi-systable klog-buf utils-printh klog-nl
  LIT" [*] uefi: handle is " klog uefi-handle klog-buf utils-printh klog-nl
  LIT" [*] uefi: oem: " klog uefi-systable s-uefi-systable/firmware-vendor klog-buf utils-print16 klog-nl

  uefi-systable s-uefi-systable/boot-services to uefi-srv-boot
  uefi-systable s-uefi-systable/runtime-services to uefi-srv-runtime

  ( get memory map, make bogus call to get memory map size )
  v' uefi-memmap-size uefi-memmap-ptr v' uefi-memmap-key v' uefi-memmap-desc-size v' uefi-memmap-desc-ver
    uefi-srv-boot s-uefi-srv-boot/get-memory-map uefi-call $ff and 2 != if
      LIT" uefi: cannot determine memory map size" panic
    then

  ( make space for 2 segmentations )
  uefi-memmap-size uefi-memmap-desc-size 2 * + to uefi-memmap-size

  ( allocate space )
  2 uefi-memmap-size v' uefi-memmap-ptr 0 0 0
    uefi-srv-boot s-uefi-srv-boot/allocate-pool uefi-call if
      LIT" uefi: cannot allocate buffer for memory map" panic
    then

  LIT" [*] uefi: memory map is at " klog uefi-memmap-ptr klog-buf utils-printh klog-nl

  begin again
;
