: serial-init
  $00 $03f9 port-out
  $80 $03fb port-out
  $03 $03f8 port-out
  $00 $03f9 port-out
  $03 $03fb port-out
  $c7 $03fa port-out
  $0b $03fc port-out
  $0f $03fc port-out ;

: serial-in
  begin
    $03fd port-in 1 and
  until $03f8 port-in ;

: serial-out
  begin
    $03fd port-in $20 and
  until $03f8 port-out ;
