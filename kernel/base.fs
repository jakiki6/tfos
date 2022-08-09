include kernel/buf.fs
include kernel/mem.fs
include kernel/sched.fs
include kernel/debug.fs
include kernel/consts.fs
include kernel/rng.fs
include kernel/font.fs
include kernel/tty.fs
include kernel/str.fs
include kernel/logo.fs
include kernel/utils.fs

: base-init
  rng-init
  tty-init
;
