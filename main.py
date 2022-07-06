#!/bin/env python3
import sys, os
import comp

cwd = os.getcwd()
os.chdir(os.path.dirname(__file__))

if len(sys.argv) < 3:
    print(sys.argv[0], "<file>", "<output>")
    exit(1)

with open(sys.argv[1], "r") as ibuf:
    content = comp.compile(ibuf.read())

os.chdir(cwd)
with open(sys.argv[2], "wb") as obuf:
    obuf.write(content)
