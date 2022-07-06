(
  structure:
    read
    write
    tell
    seek
    truncate
)

: buf-read @ execute ;
: buf-write 8 + @ execute ;
: buf-tell 16 + @ execute ;
: buf-seek 24 + @ execute ;
: buf-truncate 32 + @ execute ;
