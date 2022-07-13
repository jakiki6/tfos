: debug-dump ( addr -- )
  $0a serial-out

  dup 256 + swap
  begin
    dup c@ dup
      $0f and consts-hextable + c@ serial-out
      4 >> $0f and consts-hextable + c@ serial-out
    1+

    over over - 7 and 0 = if
      $0a serial-out
    then
  over over = until
  drop drop
;
