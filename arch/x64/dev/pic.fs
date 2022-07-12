: dev-pic-init
  $11 $20 port-out ( start init sequence )
  $11 $a0 port-out
  $80 $21 port-out ( offset )
  $88 $a1 port-out
  $04 $21 port-out ( tell them their config )
  $02 $a1 port-out
  $01 $21 port-out ( mode )
  $01 $a1 port-out 
  $fe $21 port-out
  $ff $a1 port-out
;
