import sys, os

def b(s):
    return bytes.fromhex(s.lower())

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
