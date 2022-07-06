from .x64.backend import backend
from backends import word

imms = {}

def init(binary, imm, dict):
    apply(imm)

def comment(buf, binary, imm, dict, links, back):
    while word(buf) != ")":
        continue
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

def apply(imm):
    for k, v in imms.items():
        imm[k] = v
