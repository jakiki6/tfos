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
  dup $0a = if
    0 to tty-x
    tty-y font-sy + fb-info-height % to tty-y
    drop exit
  then

  dup $0d = if
    0 to tty-x
    drop exit
  then

  dup $09 = if
    tty-x font-sx 1- + font-sx / font-sx * fb-info-height % to tty-x
    drop exit
  then

  tty-x tty-y font-render
  tty-x font-sx + to tty-x

  tty-x font-sx + fb-info-width > if
    0 to tty-x
    tty-y font-sy + fb-info-height % to tty-y
  then
;
