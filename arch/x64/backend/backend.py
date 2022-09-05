import io

from backends import *
from . import immeds

@register
class X64Backend(Backend):
    name = "x64"
    base = 0x200000
    strings = get_strings(name)

    def init(self, binary, imm, dict):
        binary.base = 0

        binary.write(self.strings["_start"])
        to_patch = binary.tell()

        for k, v in self.strings.items():
            if k.startswith("_"):
                continue

            dict[k] = binary.tell()
            binary.write(v)

        p = binary.tell()
        binary.seek(to_patch - 4)
        binary.write(i(p - to_patch, 4))
        binary.seek(p)

        immeds.apply(imm)

    def compile_num(self, binary, n, force_big=False, rel=False):
        if rel:
            binary.write(b("488D05"))
            binary.write((n - binary.tell() - 4).to_bytes(4, "little", signed=True))
            binary.write(b("48894500"))
        elif -2**31 <= n < 2 ** 31 and not force_big:
            binary.write(b("48C74500"))
            binary.write(n.to_bytes(4, "little", signed=True))
        else:
            binary.write(b("48B8"))
            binary.write(n.to_bytes(8, "little"))
            binary.write(b("48894500"))

        binary.write(b("4883C508"))

    def compile_ref(self, binary, ref, force_big=True):
        if not force_big:
            offset = ref - binary.tell() - 5
            if not (offset < (-2 ** 31) or offset >= (2 ** 31)):
                binary.write(b("E8"))
                binary.write(offset.to_bytes(4, "little", signed=True))
                return

        binary.write(b("488D05"))
        binary.write((ref - binary.tell() - 4).to_bytes(4, "little", signed=True))
        binary.write(b("FFD0"))

    def link(self, binary, addr, ref):
        p = binary.tell()
        binary.seek(addr)

        binary.write(b("488D05"))
        offset = int.from_bytes(binary.read(4), "little", signed=True)
        binary.seek(binary.tell() - 4)
        binary.write((ref + offset).to_bytes(4, "little", signed=True))

        binary.seek(p)

    def wrap(self, data):
        binary = io.BytesIO()
        if len(data) % 4096:
            data += bytes(4096 - (len(data) % 4096))

        # dos stub
        binary.write(i(0x00005a4d, 4))
        binary.write(bytes(4 * 14))
        binary.write(i(0x00000080, 4))
        binary.write(bytes(4 * 16))

        # pe coff
        binary.write(b"PE\x00\x00")     # signature
        binary.write(b("6486"))         # type
        binary.write(i(1, 2))           # sections
                                        # timestamp
        binary.write(i(0xbac8531e, 4))
        binary.write(i(0, 8))
        binary.write(i(240, 2))         # size of optional header
        binary.write(i(0x202e, 2))      # characteristics

        # optional header
        binary.write(i(0x020b, 4))      # signature
        binary.write(i(len(data), 4))   # code size
        binary.write(i(0, 4))           # data size
        binary.write(i(0, 4))           # bss size
        binary.write(i(0x1000, 4))      # entry
        binary.write(i(0, 4))           # code base
                                        # image base
        binary.write(i(self.base - 0x1000, 8))
        binary.write(i(4096, 4))        # alignments
        binary.write(i(4096, 4))
        binary.write(bytes(16))         # reserved
        binary.write(i(len(data) + 4096, 4))   # image size
        binary.write(i(4096, 4))        # headers size
        binary.write(i(0, 4))           # checksum
        binary.write(i(0x0040000a, 4))  # characteristiscs and subsystem
        binary.write(i(0x10000, 8))     # stack and heap
        binary.write(i(0x10000, 8))
        binary.write(i(0x10000, 8))
        binary.write(i(0, 8))
        binary.write(i(0, 4))           # loader flags
        binary.write(i(0x10, 4))        # rva

        # dirs
        binary.write(bytes(8 * 16))

        # sections
        binary.write(b".text\x00\x00\x00")
        binary.write(i(len(data), 4))   # size
        binary.write(i(0x1000, 4))      # address
        binary.write(i(len(data), 4))
        binary.write(i(0x1000, 4))      # offset in file
        binary.write(bytes(12))         # relocations
        binary.write(i(0xe0000020, 4))  # characteristics

        '''
        binary.write(b".data\x00\x00\x00\x00")
        binary.write(i(0, 4))           # size
        binary.write(i(0, 4))           # address
        binary.write(i(0, 4))
        binary.write(i(0, 4))           # offset in file
        binary.write(bytes(12))         # relocations
        binary.write(i(0xc0000040, 4))  # characteristics

        binary.write(b".reloc\x00\x00\x00")
        binary.write(i(0, 4))           # size
        binary.write(i(0, 4))           # address
        binary.write(i(0, 4))
        binary.write(i(0, 4))           # offset in file
        binary.write(bytes(12))         # relocations     
        binary.write(i(0x02000040, 4))  # characteristics'''

        # align
        binary.write(bytes(4096 - binary.tell()))

        # code
        binary.write(data)

        binary.seek(0)
        return binary.read()
