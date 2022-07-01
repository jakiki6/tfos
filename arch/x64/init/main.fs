$b8000 $10000 !

: foo $1f731f6f1f661f74 $10000 @ ! $10000 @ 160 + $10000 ! ;
foo

1 if foo else foo foo then

arch-hlt
