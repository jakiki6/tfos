val rng-seed

: rng-init
  arch-rng to rng-seed
;

: rng
  rng-seed
    dup 12 >> xor
    dup 25 << xor
    dup 27 >> xor
    $2545f4914f6cdd1d *
  dup to rng-seed
;
