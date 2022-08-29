(
  structure:
    u64 next
    u64 prev
    u64 ptr

  doubly linked list
)

: ar-new
  24 mem-alloc ( next, prev and ptr are already zeroed )
;

: ar-last
  dup @ 0 = if exit then

  begin
    @ dup @ 0 =
  until
;

: ar-add ( ptr ar -- )
  ar-last

  24 mem-alloc
    2dup 8 + !
    2dup swap !
  swap drop

  16 + !
;

: ar-get ( ar index )
  dup 0 = if 16 + @ exit then

  do @ loop
  16 + @
;

: _ar-iter-next ( iter -- ptr )
	dup 16 + @ dup
      @ rot< swap 16 + !
      16 + @
;

: _ar-iter-isdone ( iter -- f )
  @ 0 =
;

: ar-iter ( ar -- iter )
  24 mem-alloc
   dup ' _ar-iter-next swap !
   dup ' _ar-iter-isdone swap !
   dup rot> 16 + !
;

: ar-len ( ar -- len )
  dup @ 0 = if drop 0 then

  0
  begin
    1+ swap @ swap
    over 0 =
  until
;
