val cpu-mem-e280-head
val cpu-mem-e280-count

: cpu-mem-init
  boot-handover 8 + @ to cpu-mem-e280-head
  boot-handover 16 + w@ to cpu-mem-e280-count

  0
  cpu-mem-e280-count >r begin
    r@ 24 * cpu-mem-e280-head +
      dup @ swap 8 + @ + 12 >> max
  next

  to mem-highest

  0
  cpu-mem-e280-count >r begin
    r@ 24 * cpu-mem-e280-head +
      dup 16 + d@ 1 = if
        dup 8 + @ 3 << mem-highest > if
          swap
        then drop
      then
  next

  dup 8 + @ mem-highest 3 >> - over 8 + !
  @

  to mem-bitmap

  mem-highest 3 >> >r begin
    0 r@ mem-bitmap + c!
  next

  cpu-mem-e280-count >r begin
    r@ 24 * cpu-mem-e280-head +
      dup 16 + d@ 1 = if
        dup 8 + @ >r @ begin dup
          3 >> $ff swap c!
          1+
        next drop
      then
  next
;

: cpu-mem-dump
  cpu-mem-e280-count >r begin
    cpu-mem-e280-count r@ - 24 * cpu-mem-e280-head +
      dup @ debug-int# $20 serial-out 8 +
      dup @ debug-int# $20 serial-out 8 +
      d@ debug-int
  next
;
