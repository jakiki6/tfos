val cpu-mem-e820-head
val cpu-mem-e820-count
val cpu-mem-e820-tmp

: cpu-mem-init
  boot-handover 8 + @ to cpu-mem-e820-head
  boot-handover 16 + w@ to cpu-mem-e820-count

  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +

    dup 16 + d@ 1 = if
      dup @ swap 8 + @ +
      cpu-mem-e820-tmp max to cpu-mem-e820-tmp
    then drop
  loop

  cpu-mem-e820-tmp $7fff + 15 >> $fff + 12 >> 12 << to cpu-mem-e820-tmp
  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +

    dup @ 0 != if
      dup 16 + d@ 1 = if
        dup 8 + @ cpu-mem-e820-tmp >= if
          mem-mm-ptr 0 = if
            dup @ to mem-mm-ptr
	        dup @ cpu-mem-e820-tmp + over !
            dup 8 + @ cpu-mem-e820-tmp - over 8 + !
	      then drop
        then drop
      then drop
    then drop
  loop
;

: cpu-mem-dump
  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +
      dup @ debug-int# $20 dev-serial-out 8 +
      dup @ debug-int# $20 dev-serial-out 8 +
      d@ debug-int
  loop
;
