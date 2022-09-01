from .x64.backend import backend
from backends import word

class struct_field(object):
    def __init__(self, offset):
        self.offset = offset

    def __call__(self, _buf, _binary, _imm, _dict, _links, _backend):
        _backend.compile_num(_binary, self.offset)
        _backend.compile_ref(_binary, _dict["+"])

imms = {}

def init(binary, imm, dict):
    apply(imm)

def comment(buf, binary, imm, dict, links, back):
    level = 1
    while level:
        w = word(buf)
        if w == "(":
            level += 1
        elif w == ")":
            level -= 1
imms["("] = comment

def include(buf, binary, imm, dict, links, back):
    filename = word(buf)
    with open(filename, "r") as file:
        content = file.read() + "\n" + buf.read()
        buf.seek(0)
        buf.write(content)
        buf.truncate()
        buf.seek(0)
imms["include"] = include

def struct(buf, binary, imm, dict, links, back):
    name = word(buf)

    assert word(buf) == "{"

    offset = 0
    while True:
        field = word(buf)

        if field == "}":
            break

        length = word(buf)
        if length == "*":
            length = 8
        else:
            length = int(length)

        imm[f"{name}/{field}"] = struct_field(offset)
        offset += length
imms["struct"] = struct

def apply(imm):
    for k, v in imms.items():
        imm[k] = v
