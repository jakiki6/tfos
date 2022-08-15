: min ( a b -- c )
  over over > if swap then drop
;

: max ( a b -- c )
  over over < if swap then drop 
;

: utils-printn ( n buf -- )
  ( I know recursion is bad but the maximal recursion depth you can get is 20 )

  0 ( placeholder )
  b[
    2 # @ 10 /%

	over if
      over 1 # @ utils-printn
    then swap drop

    $30 + 0 # !
    0 # 1 1 # @ buf-write drop

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
