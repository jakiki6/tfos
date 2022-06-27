import os

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

@register
class X64Backend(Backend):
    name = "x64"
    base = 0x10000
    strings = get_strings(name)

    def init(self, binary, imm, dict):
        binary.base = self.base

        # stable abi
        binary.write(b("90909090909048B8"))
        binary.write(b("0000000000000000"))
        binary.write(b("FFE0000000000000"))

        for k, v in self.strings.items():
            dict[k] = binary.tell()
            binary.write(v)

        p = binary.tell()
        binary.seek(8, 4)
        binary.write(p.to_bytes(8, "little"))
        binary.seek(p)

    def compile_num(self, binary, n):
        if n < 2 ** 8:
            binary.write(b("4831C0B0"))
            binary.write(n.to_bytes(1, "little"))
        elif n < 2 ** 16:
            binary.write(b("4831C066B8"))
            binary.write(n.to_bytes(2, "little"))
        elif n < 2 ** 32:
            binary.write(b("4831C0B8"))
            binary.write(n.to_bytes(4, "little"))
        else:
            binary.write(b("48B8"))
            binary.write(n.to_bytes(8, "little"))

        binary.write(b("488945004883C508"))

    def compile_ref(self, binary, ref):
        binary.write(b("48B8"))
        binary.write(ref.to_bytes(8, "little"))
        binary.write(b("FFD0"))
