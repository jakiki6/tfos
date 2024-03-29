: debug-hex-one ( i -- )
  dup
    4 >> $0f and consts-hextable + c@ dev-serial-out
    $0f and consts-hextable + c@ dev-serial-out
;

: debug-int# ( i -- )
  bswap
    8 >r begin
      dup debug-hex-one
      8 >>
    next drop
;

: debug-int ( i -- )
  debug-int#
  $0a dev-serial-out
;

: debug-dump ( addr -- )
  $0a dev-serial-out

  dup 256 + swap
  begin
    dup
      c@ debug-hex-one
    1+
    over over - 7 and 0 = if
      $0a dev-serial-out
    then
  over over = until
  drop drop
;

: debug-stack ( -- )
  8 do
    debug-int
  loop
  begin again
;
