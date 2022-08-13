(
  structure:
    read ( dest count -- read )
    write ( src count -- written )
    tell ( -- offset )
    seek ( offset -- )
    truncate ( len -- )
)

: buf-read @ execute ;
: buf-write 8 + @ execute ;
: buf-tell 16 + @ execute ;
: buf-seek 24 + @ execute ;
: buf-truncate 32 + @ execute ;

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
