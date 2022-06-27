#!/bin/env python3
import sys
import comp

if len(sys.argv) < 3:
    print(sys.argv[0], "<file>", "<output>")
    exit(1)

with open(sys.argv[1], "r") as ibuf:
    with open(sys.argv[2], "wb") as obuf:
        obuf.write(comp.compile(ibuf.read()))
