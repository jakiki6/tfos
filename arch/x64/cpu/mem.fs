val cpu-mem-e820-head
val cpu-mem-e820-count

: cpu-mem-init
  boot-handover 8 + @ to cpu-mem-e820-head
  boot-handover 16 + w@ to cpu-mem-e820-count
;

: cpu-mem-dump
  cpu-mem-e820-count >r begin
    cpu-mem-e820-count r@ - 24 * cpu-mem-e820-head +
      dup @ debug-int# $20 dev-serial-out 8 +
      dup @ debug-int# $20 dev-serial-out 8 +
      d@ debug-int
  next
;
