val fb
$b8000 to fb

: print begin
  dup c@ fb c! fb 1+ $1f c! fb 2 + to fb 1+ dup c@ 0 =
until ;

LIT" tfos" print

val color
begin $b8140
  begin dup color swap c! 1 + dup $b8fa0 = until
drop color
  if 0 to color
  else rng to color
  then
again

arch-hlt
