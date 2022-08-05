val tty-x
val tty-y
val tty-rows
val tty-cols

: tty-init
  0 to tty-x
  0 to tty-y

  fb-info-height font-sy / to tty-rows
  fb-info-width font-sx / to tty-cols
;

: tty-write-one ( c -- )
(  dup $0a = if
    0 to tty-x
    tty-y 1 + tty-rows % to tty-y
    drop exit
  then

  dup $0d = if
    0 to tty-x
    drop exit
  then

  dup $09 = if
    tty-x 7 + 8 / 8 * tty-cols % to tty-x
    drop exit
  then )

  tty-x font-sx * tty-y font-sy * font-render
  tty-x 1+ to tty-x

  ( tty-x tty-cols < not if
    0 to tty-x
    tty-y 1 + tty-rows % to tty-y
  then )
;
