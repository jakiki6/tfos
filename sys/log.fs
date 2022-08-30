: klog
  tty-buf buf-print drop
;

: klog-c
  tty-buf buf-write1
;

: klog-w
  tty-buf buf-write drop
;

: klog-buf
  tty-buf
;
