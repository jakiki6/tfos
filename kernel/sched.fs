val sched-head

: sched-init
  16 mem-alloc to sched-head
  0 sched-head !
  sched-head sched-head 8 + !
;

: sched-handler
  sched-head !
  sched-head 8 + @ to sched-head
  sched-head @
;

: sched-add ( addr -- )
  cpu-irq-disable
  cpu-sched-add
  16 mem-alloc dup
    sched-head 8 + dup @ rot> !
    swap to sched-head
    sched-head 8 + !
    sched-head !
  cpu-irq-enable
;
