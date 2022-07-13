: cpu-sched-init
  $80
  $1000 mem-alloc
  ' cpu-sched-handler swap dup $ff8 + arch-int-timer
  1 cpu-idt-register ;

: cpu-sched-handler sched-handler ;

: cpu-sched-add ( addr -- frame )
  $1000 mem-alloc
    dup $ff8 + ( addr ps rs )
    dup 4 >> 4 << ( align to 16 bytes ) ( addr ps rs rs' )
    over $10 ! 8 - ( ss )
    over over ! 8 - ( rsp )
    swap drop ( addr ps rs' )
    over $0200 ! 8 - ( rflags )
    over $08 ! 8 - ( cs )
    rot< over ! 8 - ( rip ) ( ps rs' )
    48 - ( skip rdi, rsi, rdx, rcx, rbx and rax )
    over over ! ( rbp )
    over drop ( rs' )
(    dup debug-dump )
;
