: str-len ( str -- len )
  0
  begin
    1+ swap 1+ swap over c@ not
  until
  swap drop
;

: str-len16 ( str -- len )
  0
  begin
    1+ swap 2 + swap over c@ not
  until
  swap drop
;
