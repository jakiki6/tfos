val uefi-systable
val uefi-handle

val uefi-srv-boot
val uefi-srv-runtime

: uefi-init
  LIT" [*] uefi systable: " klog uefi-systable klog-buf utils-printh klog-nl
  LIT" [*] uefi oem: " klog uefi-systable s-uefi-systable/firmware-vendor klog-buf utils-print16 klog-nl

  uefi-systable s-uefi-systable/boot-services to uefi-srv-boot
  uefi-systable s-uefi-systable/runtime-services to uefi-srv-runtime
;
