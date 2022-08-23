: logo-print
  fb-info-width logo-sx - 1 >>
  fb-info-height logo-sy - 1 >>
  b[
    logo-sy do
      logo-sx do
        j logo-sx * i + 3 * logo-blob +
        dup c@ swap 1+ dup c@ swap 1+ c@
        i 2 # @ +
        j 1 # @ +
        fb-put
      loop
    loop
  ]b
;

include kernel/logo_bin.fs
