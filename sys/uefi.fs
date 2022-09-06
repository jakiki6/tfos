val uefi-systable
val uefi-handle

val uefi-srv-boot
val uefi-srv-runtime

: uefi-init
  LIT" [*] uefi: systable is at " klog uefi-systable klog-buf utils-printh klog-nl
  LIT" [*] uefi: handle is " klog uefi-handle klog-buf utils-printh klog-nl
  LIT" [*] uefi: oem: " klog uefi-systable s-uefi-systable/firmware-vendor klog-buf utils-print16 klog-nl

  uefi-systable s-uefi-systable/boot-services to uefi-srv-boot
  uefi-systable s-uefi-systable/runtime-services to uefi-srv-runtime

  uefi-handle 0 0 0
    uefi-srv-boot s-uefi-srv-boot/exit uefi-call

  LIT" [*] uefi: exit call returned " klog klog-buf utils-printh klog-nl
;
