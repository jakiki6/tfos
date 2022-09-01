: min ( a b -- c )
  over over > if swap then drop
;

: max ( a b -- c )
  over over < if swap then drop 
;

: 2dup ( a b -- a b a b )
  over over
;

: 2drop ( a b -- )
  drop drop
;

: utils-printn ( n buf -- )
  ( I know recursion is bad but the maximal recursion depth you can get is 20 )

  b[
    0 ( placeholder )

    2 # @ 10 /%

	over if
      over 1 # @ utils-printn
    then swap drop

    $30 + -1 # !
    -1 # 1 1 # @ buf-write drop

    3 leave
  ]b
;

: utils-printh1 ( n buf -- )
  over 4 >> consts-hextable + c@ over buf-write1
  over $0f and consts-hextable + c@ over buf-write1

  drop drop
;

: utils-printh ( n buf -- )
  8 >r begin
    over 56 >> over utils-printh1
    swap 8 << swap
  next

  drop drop
;

: utils-printc ( c buf -- )
	buf-write1
;

: utils-print16 ( str buf -- )
  swap dup str-len16 do
    2dup c@ swap buf-write1
    2 +
  loop

  2drop
;