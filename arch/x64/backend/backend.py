from backends import *
from . import immeds

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

        immeds.apply(imm)

        p = binary.tell()
        binary.seek(8, 4)
        binary.write(p.to_bytes(8, "little"))
        binary.seek(p)

    def compile_num(self, binary, n):
        if -2**31 <= n < 2 ** 31:
            binary.write(b("48C74500"))
            binary.write(n.to_bytes(4, "little", signed=True))
        else:
            binary.write(b("48B8"))
            binary.write(n.to_bytes(8, "little"))
            binary.write(b("48894500"))

        binary.write(b("4883C508"))

    def compile_ref(self, binary, ref):
        if ref != 0:
            offset = ref - binary.tell() - 5
            if not (offset < (-2 ** 31) or offset >= (2 ** 31)):
                binary.write(b("E8"))
                binary.write(offset.to_bytes(4, "little", signed=True))
                return

        binary.write(b("48B8"))
        binary.write(ref.to_bytes(8, "little"))
        binary.write(b("FFD0"))
