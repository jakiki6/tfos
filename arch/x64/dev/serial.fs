val dev-serial-buf

: dev-serial-init
  $00 $03f9 port-out
  $80 $03fb port-out
  $03 $03f8 port-out
  $00 $03f9 port-out
  $03 $03fb port-out
  $c7 $03fa port-out
  $0b $03fc port-out
  $0f $03fc port-out

  bss-alloc 40
    to dev-serial-buf
    ' dev-serial-buf-read dev-serial-buf 0 + !
    ' dev-serial-buf-write dev-serial-buf 8 + !
    ' dev-serial-buf-tell dev-serial-buf 16 + !
    ' dev-serial-buf-seek dev-serial-buf 24 + !
    ' dev-serial-buf-truncate dev-serial-buf 32 + !
;

: dev-serial-in
  begin
    $03fd port-in 1 and
  until $03f8 port-in ;

: dev-serial-out
  begin
    $03fd port-in $20 and
  until $03f8 port-out ;

: dev-serial-buf-read ( dest count buf -- read )
  drop drop drop 0
;

: dev-serial-buf-write ( src count buf -- written )
  drop swap   
  over do
    dup c@ dev-serial-out
    1+
  loop drop   
;

: dev-serial-buf-tell ( buf -- offset )
  drop
  tty-y tty-cols * tty-x +
;

: dev-serial-buf-seek ( offset buf -- )
  drop drop
;

: dev-serial-buf-truncate ( len buf -- )
  drop drop
;
