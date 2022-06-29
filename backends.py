import sys, os

def b(s):
    return bytes.fromhex(s.lower())

def skip(buf):
    while True:
        c = buf.read(1)
        if len(c) == 0:
            break

        if not c in " \t\n":  
            buf.seek(buf.tell() - 1)
            break

def word(buf):
    skip(buf)

    w = ""
    while True:
        c = buf.read(1)

        if c in " \t\n":
            break

        w += c

    return w

class Backend(object):
    def __init__(self):
        pass

    def init(self, binary, imm, dict):
        pass

    def compile_num(self, binary, n):
        pass

    def compile_ref(self, binary, ref):
        pass

backends = {}
def register(cls):
    backends[cls.name] = cls()

    return cls

def get_strings(name):
    strings = {}

    with open(os.path.join(os.path.dirname(__file__), "arch", name, "backend", "strings.txt"), "r") as file:
        for line in file.read().split("\n"):
            if len(line.strip()) == 0:
                continue

            strings[line.split(" ")[0]] = b(line.split(" ")[1]) 

    return strings

basedir = os.path.dirname(__file__)
sys.path.append(basedir)
import arch
