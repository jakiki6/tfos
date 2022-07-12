: cpu-sched-init
  $80
  $4000 mem-alloc
  ' cpu-sched-handler swap dup $4000 + arch-int-timer
  1 cpu-idt-register ;

: cpu-sched-handler ;
