import io, os
import backends, arch

class BinaryBuffer(object):
    def __init__(self, content=b""):
        self.buf = io.BytesIO(content)
        self.base = 0

    def read(self, size=-1):
        return self.buf.read(size)

    def write(self, b):
        return self.buf.write(b)

    def tell(self):
        return self.buf.tell() + self.base

    def seek(self, pos, whence=0):
        if whence == 4:
            whence = 0
        elif whence != 1:
            pos -= self.base

        return self.buf.seek(pos, whence)

    def reset(self):
        self.buf.seek(0)

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

def compile(content, backend=backends.backends["x64"]):
    buf = io.StringIO(content)
    binary = BinaryBuffer()
    imm = {}
    dict = {}
    links = {}

    arch.init(binary, imm, dict)
    backend.init(binary, imm, dict)

    while True:
        w = word(buf)
        if len(w) == 0:
            break

        try:
            _w = w
            base = 10
            if _w[0] == "$":
                _w = _w[1:]
                base = 16
            elif _w[0] == "%":
                _w = _w[1:]
                base = 2

            n = int(_w, base)
            if n < 0:
                n += 2 ** 64

            if n > (2 ** 64) or n < 0:
                print(f"Number '{n}' is too small/big")
                exit(1)

            backend.compile_num(binary, n)
        except ValueError:
            if w in imm:
                imm[w](buf, binary, imm, dict, links, backend)
            elif w in dict:
                backend.compile_ref(binary, dict[w])
            else:
                links[binary.tell()] = (w, "ref")
                backend.compile_ref(binary, 0, force_big=True)

    for addr, link in links.items():
        if not link[0] in dict:
            print(f"Unknown reference to word '{link[0]}'")
            exit(1)

        backend.link(binary, addr, dict[link[0]], link[1])

    if "DEBUG" in os.environ:
        for k, v in dict.items():
            print(k, hex(v))

    binary.reset()
    return backend.wrap(binary.read())
