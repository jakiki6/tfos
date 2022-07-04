include dev/serial.fs

val fb
$b8000 to fb

serial-init
: print begin
  dup c@ dup serial-out fb c! fb 1+ $1f c! fb 2 + to fb 1+ dup c@ not
until ;

LIT" booting tfos..." print

$f0000 $b8140 480 mem-copy
val ptr
$ffff5 to ptr
$b8078 to fb
8 begin ptr c@ fb c! ptr 1+ to ptr fb 2 + to fb 1- dup 0 = until drop

begin
  cookie-push
  $100
  begin $b8320
    begin dup rng swap c! 1+ dup $b8fa0 = until
  drop 1- dup not until drop

  $b8090 @ 1+ $b8090 !
  cookie-pop
again
