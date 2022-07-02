$b8000 $10000 !

: foo $1f731f6f1f661f74 $10000 @ ! $10000 @ 160 + $10000 ! ;
foo

begin $b8140
  begin dup dup c@ rng + swap c! 1 + dup $b8fa0 = until
drop again

arch-hlt
