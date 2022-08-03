val rng-seed

: rng-init
  arch-rng to rng-seed
;

: rng
  rng-seed $5deece66d * $b + dup to rng-seed
  16 >>
;
