val tty-x
val tty-y
val tty-rows
val tty-cols

val tty-buf

: tty-init
  0 to tty-x
  0 to tty-y

  fb-info-height font-sy / to tty-rows
  fb-info-width font-sx / to tty-cols

  40 mem-alloc
    to tty-buf
    ' tty-buf-read tty-buf 0 + !
    ' tty-buf-write tty-buf 8 + !
    ' tty-buf-tell tty-buf 16 + !
    ' tty-buf-seek tty-buf 24 + !
    ' tty-buf-truncate tty-buf 32 + !
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

: tty-buf-read ( dest count buf -- read )
  drop drop drop 0
;

: tty-buf-write ( src count buf -- written )
  drop swap
  over do
    dup c@ tty-write-one
    1+
  loop drop
;

: tty-buf-tell ( buf -- offset )
  drop
  tty-y tty-cols * tty-x +
;

: tty-buf-seek ( offset buf -- )
  drop
  tty-cols /%
    to tty-x
    to tty-y
;

: tty-buf-truncate ( len buf -- )
  drop drop
;
