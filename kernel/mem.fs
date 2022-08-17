val mem-ptr
$200000 to mem-ptr

val mem-mm-ptr
val mem-mm-highest
val mem-mm-last

( : mem-alloc mem-ptr swap over + to mem-ptr ; )
: mem-alloc $fff + 12 >> mem-mm-alloc ;
: mem-free drop ;

: mem-mm-alloc ( pages -- addr ) 
  b[
    0 ( current address )
    0 # @ ( left to find )

    begin
      ( overflow? )
      -1 # @ mem-mm-highest = if
        - -1 # !
      then

      -1 # @ 3 >> mem-mm-ptr + c@ ( get byte )
      -1 # @ %111 and >> 1 and ( get bit )
      if
        ( decrement by one )
        -2 # @ 1- -2 !
      else
        ( reset streak )
        1 # @ -2 # !
      then

      0 # @ not
    until

    1 # - ( get start )

    ( clear bits )
    1 # do
      dup i + dup dup
        3 >> mem-mm-ptr + c@
        swap %111 and 1 swap << $ff xor and
        swap c!
    loop

    swap
  ]b

  swap drop
;
