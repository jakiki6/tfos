$b8000 $10000 !

: foo $1f731f6f1f661f74 $10000 @ ! $10000 @ 160 + $10000 ! ;
foo

0 $b8143 c!
begin $b8140 @ 1 + $b8140 ! $b8143 c@ until

arch-hlt
