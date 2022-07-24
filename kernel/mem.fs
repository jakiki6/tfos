val mem-ptr
$200000 to mem-ptr

: mem-alloc mem-ptr swap over + to mem-ptr ;
: mem-free drop ;
