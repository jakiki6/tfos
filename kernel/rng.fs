val rng-seed

: rng-init
  arch-rng to rng-seed
;

: rng-int
  rng-seed
    $2545f4914f6cdd1d *
    dup 12 >> xor
    dup 25 << xor
    dup 27 >> xor
  dup to rng-seed
;

: rng-bool
  rng-int 1 and
;

: rng-neg
  rng-bool if
    0 swap -
  then
;

: rng-pm1
  rng-bool rng-neg
;
