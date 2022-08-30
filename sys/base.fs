include lib/base.fs

include sys/mem.fs
include sys/sched.fs
include sys/debug.fs
include sys/consts.fs
include sys/rng.fs
include sys/font.fs
include sys/tty.fs
include sys/panic.fs
include sys/log.fs

: base-init
  rng-init
  tty-init
;
