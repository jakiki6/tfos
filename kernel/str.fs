: str-len ( str -- len )
  0
  begin
    1+ swap 1+ swap over c@ not
  until
  1- swap drop
;
