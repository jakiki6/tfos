val mem-bitmap
val mem-highest

: mem-page-alloc ( count -- )
  0 0 ( addr c )
  begin
    over dup 7 and swap 3 >> c@ swap >> 1 and
      if 1+ else drop 0 then
    rot< over over = if drop drop 12 << exit then rot< 4096 + rot<
  dup mem-highest > until
  0
;

: mem-alloc ( size -- )
  12 >> 1+ mem-page-alloc
;

: mem-free ;
