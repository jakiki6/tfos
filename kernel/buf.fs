(
  structure:
    read ( dest count buf -- read )
    write ( src count buf -- written )
    tell ( buf -- offset )
    seek ( offset buf -- )
    truncate ( len buf -- )
)

: buf-read dup @ execute ;
: buf-write 8 + dup @ execute ;
: buf-tell 16 + dup @ execute ;
: buf-seek 24 + dup @ execute ;
: buf-truncate 32 + dup @ execute ;

: buf-seek-rel dup buf-tell + buf-seek ;

: buf-print ( str buf -- )
  swap dup str-len rot< buf-write
;

: buf-read1 ( buf -- )
  0 1 rot< srel 32 - rot> buf-read drop
;

: buf-write1 ( c buf -- )
  1 swap srel 24 - rot> buf-write drop drop
;
