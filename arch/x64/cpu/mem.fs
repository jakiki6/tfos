val cpu-mem-e280-head
val cpu-mem-e280-count

: cpu-mem-init
  boot-handover 8 + @ to cpu-mem-e280-head
  boot-handover 16 + w@ to cpu-mem-e280-count
;

( : cpu-mem-dump
  cpu-mem-e280-count >r begin
    cpu-mem-e280-count r@ - 24 * cpu-mem-e280-head +
      dup @ debug-int# $20 serial-out 8 +
      dup @ debug-int# $20 serial-out 8 +
      d@ debug-int
  next
; )
