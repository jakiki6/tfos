val fb-vesa-info
val fb-info-pitch
val fb-info-width
val fb-info-height
val fb-info-addr

: fb-init
  boot-handover @ to fb-vesa-info
  fb-vesa-info 16 + w@ to fb-info-pitch
  fb-vesa-info 18 + w@ to fb-info-width
  fb-vesa-info 20 + w@ to fb-info-height
  fb-vesa-info 40 + @ to fb-info-addr
;

: fb-draw-one ( r g b x y -- )
  fb-info-pitch * swap 3 * + fb-info-addr + 2 +
  dup rot> c! 1-
  dup rot> c! 1-
  c!
;
