val sched-head
val sched-head-main
val sched-head-idle

(
  format of a sched entry:
    u64 stack
    u64 next
    u64 waitfor [ 0 means no wait ]
    u64 waitval
)

: sched-init
  32 mem-alloc dup
    to sched-head
    to sched-head-main
  0 sched-head-main !
  sched-head-main sched-head-main 8 + !
  0 sched-head-main 16 + !

  ' sched-idle cpu-sched-add
  24 mem-alloc 
    to sched-head-idle
  sched-head-idle !
  sched-head-main sched-head-idle 8 + !
  0 sched-head-idle 16 + !
;

val _sched-head
: sched-handler
  sched-head !

  sched-head 8 + @ to sched-head
  sched-head to _sched-head
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
    dup _sched-head = drop 1
  until

  sched-head-idle to sched-head
  sched-head @
;

: sched-add ( addr -- )
  cpu-irq-disable
  cpu-sched-add
  sched-head 8 + @ swap
  32 mem-alloc
    dup 16 + 0 swap !
    dup sched-head 8 + !
    dup rot> !
    8 + !
  cpu-irq-enable
;

: sched-waitfor
  cpu-irq-disable

  sched-head 16 + !
  sched-head 24 + !

  cpu-irq-enable
  cpu-sched-wait
;

: sched-idle begin arch-pause again ;
