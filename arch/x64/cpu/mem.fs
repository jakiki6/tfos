val cpu-mem-e820-head
val cpu-mem-e820-count
val cpu-mem-e820-tmp

: cpu-mem-init
  boot-handover 8 + @ to cpu-mem-e820-head
  boot-handover 16 + w@ to cpu-mem-e820-count

  ( remove kernel range from memmap )
  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +

    dup @ $100000 >= if ( bigger then lower bound )
      dup @ $200000 < if ( smaller than upper bound aka start is in kernel range )
        dup @ $200000 swap - ( get size to be removed )
        over 8 + @ rot< - over 8 + ! ( decrease size )
        dup $200000 swap ! ( write new base )
      then drop
    then drop
  loop

  ( get highest page )
  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +

    dup 16 + d@ 1 = if
      dup @ swap 8 + @ +
      cpu-mem-e820-tmp max to cpu-mem-e820-tmp
    then drop
  loop

  ( find suitable range for bitmap and decrease entry )
  cpu-mem-e820-tmp $fff 12 >> to mem-mm-highest
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

  mem-mm-ptr 0 = if
    arch-hlt
  then

  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +

    dup @ 0 != if
      dup 16 + d@ 1 = if
	    dup 8 + @ $fff + 12 >> swap @ $fff + 12 >> swap do
          dup i +
            dup dup 3 >> mem-mm-ptr + c@ swap %111 and 1 swap << or swap 3 >> mem-mm-ptr + c!
        loop
      then drop
    then drop
  loop

  ( protect kernel )
  mem-mm-ptr $20 + $20 mem-wipe

  1 mem-alloc debug-int arch-hlt
;

: cpu-mem-dump
  cpu-mem-e820-count do
    i 24 * cpu-mem-e820-head +
      dup @ debug-int# $20 dev-serial-out 8 +
      dup @ debug-int# $20 dev-serial-out 8 +
      d@ debug-int
  loop
;
