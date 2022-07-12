val cpu-idt-tbl
val cpu-idt-desc

: cpu-idt-init
  4096 mem-alloc to cpu-idt-tbl
  10 mem-alloc to cpu-idt-desc

  4095 cpu-idt-desc w!
  cpu-idt-tbl cpu-idt-desc 2 + !

  $00 ' arch-trap-nop 0 cpu-idt-register

  cpu-idt-desc arch-lidt arch-sti ;

: cpu-idt-register ( id addr type -- )
  rot< 4 << cpu-idt-tbl + ( base address )
  dup rot< $8e or swap 5 + c! ( type )
  over $ffff and over w! ( offset 1 )
  over 16 >> $ffff and over 6 + w! ( offset 2 )
  over 32 >> over 8 + d! ( offset 3 )

  swap drop
  dup 2 + $08 swap w! ( selector )
  dup 4 + $00 swap c! ( ist )
  14 + 0 swap d! ( zero ) ;
