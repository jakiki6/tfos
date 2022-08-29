(
  structure:
    u64 next
    u64 isdone
    u8 data[?]
)

: iter-next ( iter -- data )
  dup @ execute
;

: iter-isdone ( iter -- f )
  dup 8 + @ execute
;
