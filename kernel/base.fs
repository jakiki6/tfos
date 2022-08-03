include kernel/buf.fs
include kernel/mem.fs
include kernel/sched.fs
include kernel/debug.fs
include kernel/consts.fs
include kernel/rng.fs

: base-init
  rng-init
;