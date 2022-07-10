val cpu-idt-tbl
val cpu-idt-desc

: cpu-idt-init
  4096 mem-alloc to cpu-idt-tbl
  10 mem-alloc to cpu-idt-desc

  4095 cpu-idt-desc w!
  cpu-idt-tbl cpu-idt-desc 2 + !

  cpu-idt-desc arch-lidt ;
