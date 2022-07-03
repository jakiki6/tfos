val fb
$b8000 to fb

: foo $1f731f6f1f661f74 fb ! $10000 @ 160 + to fb ;
val uwu
' foo to uwu
uwu execute

val color
begin $b8140
  begin dup color swap c! 1 + dup $b8fa0 = until
drop color
  if 0 to color
  else $ff to color
  then
again

arch-hlt
