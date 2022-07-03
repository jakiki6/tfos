val fb
$b8000 to fb

: foo $1f731f6f1f661f74 fb ! $10000 @ 160 + to fb ;
val uwu
' foo to uwu
uwu execute


begin $b8140
  begin dup dup c@ rng + swap c! 1 + dup $b8fa0 = until
drop again

arch-hlt
