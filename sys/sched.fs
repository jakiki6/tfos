val sched-head

(
  format of a sched entry:
    u64 stack
    u64 next
    u64 waitfor [ 0 means no wait ]
    u64 waitval
    u64 prev
)

: sched-init
  40 mem-alloc
    to sched-head
  0 sched-head !
  sched-head sched-head 8 + !
  sched-head sched-head 32 + !
  0 sched-head 16 + !

  ' sched-idle sched-add
;

: sched-handler
  sched-head !

  sched-head 8 + @ to sched-head
  begin
    sched-head 16 + @
      dup 0 = if
        sched-head @ exit
      then
      @ sched-head 24 + @ = if
        0 sched-head 16 + !
        sched-head @ exit
      then

    sched-head 8 + @ to sched-head
  again
;

: sched-add ( addr -- )
  cpu-irq-disable
  cpu-sched-add
  sched-head 8 + @ swap
  40 mem-alloc
    dup 16 + 0 swap !
    dup sched-head 8 + !
    dup rot> !
    dup dup 32 + !
    8 + !
  cpu-irq-enable
;

: sched-exit
  cpu-irq-disable

  ( update the next field of the previous task )
  sched-head 8 + @ sched-head 32 + @ 8 + !

  ( update the prev field of the next task )
  sched-head 32 + @ sched-head 8 + @ 32 + !

  cpu-irq-enable

  ( we will be killed after the next timer irq so we just busy loop )
  begin arch-pause again
;

: sched-waitfor
  cpu-irq-disable

  sched-head 16 + !
  sched-head 24 + !

  cpu-irq-enable
  cpu-sched-wait
;

: sched-idle begin again ;
