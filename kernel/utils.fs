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
