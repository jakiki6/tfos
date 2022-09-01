from .x64.backend import backend
from backends import word

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

        length = int(word(buf))

        imm[f"{name}->{field}"] = lambda _buf, _binary, _imm, _dict, _links, _backend: _backend.compile_num(_binary, offset)
        offset += length
imms["struct"] = struct

def apply(imm):
    for k, v in imms.items():
        imm[k] = v
