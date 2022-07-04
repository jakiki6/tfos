val fb
$b8000 to fb

: print begin
  dup c@ fb c! fb 1+ $1f c! fb 2 + to fb 1+ dup c@ not
until ;

LIT" booting tfos..." print

$f0000 $b8140 480 mem-copy

begin
  $100
  begin $b8320
    begin dup rng swap c! 1+ dup $b8fa0 = until
  drop 1- dup not until

  $b8090 @ 1+ $b8090 !
again