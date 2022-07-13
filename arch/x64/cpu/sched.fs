: cpu-sched-init
  $80
  $1000 mem-alloc
  ' cpu-sched-handler swap dup $ff8 + arch-int-timer
  1 cpu-idt-register ;

: cpu-sched-handler sched-handler ;

: cpu-sched-add ( addr -- frame )
  $1000 mem-alloc
    dup $ff8 + ( addr ps rs )
    4 >> 4 << ( align to 16 bytes ) ( addr ps rs )
    $10 over ! 8 - ( ss )
    dup 8 + over ! 8 - ( rsp )
    $0200 over ! 8 - ( rflags )
    $08 over ! 8 - ( cs )
    rot< over ! 8 - ( rip ) ( ps rs )
    48 - ( skip rdi, rsi, rdx, rcx, rbx and rax )
    over over ! ( rbp )
    swap drop ( rs )
;
