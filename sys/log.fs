: klog
  dev-serial-buf buf-print drop
;

: klog-c
  dev-serial-buf buf-write1
;

: klog-nl
  $0a klog-c
;

: klog-w
  dev-serial-buf buf-write drop
;

: klog-buf
  dev-serial-buf
;

: klog-s
  $20 klog-c
;

: klog-h
  klog-buf utils-printh
;
