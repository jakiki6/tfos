from backends import b, word, backends
import random

imms = {}

stack = []

def define_word(buf, binary, imm, dict, links, back):
    binary.write(b("E9"))
    stack.append(binary.tell())
    binary.write(b("00000000"))
    dict[word(buf)] = binary.tell()
imms[":"] = define_word
imms["fn"] = define_word

def end_word(buf, binary, imm, dict, links, back):
    binary.write(b("C3"))
    target = stack.pop()
    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.seek(p)
imms[";"] = end_word

def scope_begin(buf, binary, imm, dict, links, back):
    back.compile_ref(binary, dict["b["])
imms["{"] = scope_begin

def scope_end(buf, binary, imm, dict, links, back):
    back.compile_ref(binary, dict["]b"])
    binary.write(b("C3"))
    target = stack.pop()
    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.seek(p)
imms["}"] = scope_end

def cond_clause(buf, binary, imm, dict, links, back):
    binary.write(b("4883ED0848837D00000F84"))
    stack.append(binary.tell())
    binary.write(b("00000000"))
imms["if"] = cond_clause

def then_clause(buf, binary, imm, dict, links, back):
    target = stack.pop()
    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.seek(p)
imms["then"] = then_clause

def else_clause(buf, binary, imm, dict, links, back):
    target = stack.pop()

    binary.write(b("E9"))
    stack.append(binary.tell())
    binary.write(b("00000000"))

    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True));
    binary.seek(p)
imms["else"] = else_clause

def begin_clause(buf, binary, imm, dict, links, back):
    stack.append(binary.tell())
imms["begin"] = begin_clause

def do_clause(buf, binary, imm, dict, links, back):
    back.compile_ref(binary, dict["dup"])
    back.compile_ref(binary, dict[">r"])
    back.compile_ref(binary, dict[">r"])
    stack.append(binary.tell())
imms["do"] = do_clause

def loop_clause(buf, binary, imm, dict, links, back):
    target = stack.pop() 
    rel = target - binary.tell() - 10
    binary.write(b("48FF0C240F85"))
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.write(b("5858")) 
imms["loop"] = loop_clause

def again_clause(buf, binary, imm, dict, links, back):
    target = stack.pop()
    rel = target - binary.tell() - 5
    binary.write(b("E9"))
    binary.write(rel.to_bytes(4, "little", signed=True))
imms["again"] = again_clause

def until_clause(buf, binary, imm, dict, links, back):
    target = stack.pop()
    rel = target - binary.tell() - 15
    binary.write(b("4883ED0848837D00000F84"))
    binary.write(rel.to_bytes(4, "little", signed=True))
imms["until"] = until_clause

def next_clause(buf, binary, imm, dict, links, back):
    target = stack.pop() 
    rel = target - binary.tell() - 10
    binary.write(b("48FF0C240F85"))
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.write(b("58"))
imms["next"] = next_clause

def val_word(buf, binary, imm, dict, links, back):
    binary.write(b("EB13"))
    dict[word(buf)] = binary.tell()
    binary.write(b("48B8"))
    binary.write(bytes(8))
    binary.write(b("488945004883C508C3"))
imms["val"] = val_word

def to_word(buf, binary, imm, dict, links, back):
    name = word(buf)
    if name in dict:
        back.compile_num(binary, dict[name] + 2, rel=True)
    else:
        links[binary.tell()] = name
        back.compile_num(binary, 2, force_big=True, rel=True)

    back.compile_ref(binary, dict["!"])
imms["to"] = to_word

def hyphen_word(buf, binary, imm, dict, links, back):
    name = word(buf)
    if name in dict:
        back.compile_num(binary, dict[name], rel=True)
    else:
        links[binary.tell()] = name
        back.compile_num(binary, 0, force_big=True, rel=True)
imms["'"] = hyphen_word

def valhyphen_word(buf, binary, imm, dict, links, back):
    name = word(buf)  
    if name in dict:
        back.compile_num(binary, dict[name] + 2, rel=True)
    else:
        links[binary.tell()] = name
        back.compile_num(binary, 2, force_big=True, rel=True)
imms["v'"] = valhyphen_word

def str_lit(buf, binary, imm, dict, links, back):
    content = ""
    escape = False

    while True:
        c = buf.read(1)

        if not escape and c == "\"":
            break
        elif not escape and c == "\\":
            escape = True
        elif escape:
            if c == "\\":
                content += c
            elif c == "n":
                content += "\n"
            elif c == "t":
                content += "\t"

            escape = False
        else:
            content += c
            escape = False

    content = content.encode("utf-8") + b"\x00"

    if len(content) < 128:
        binary.write(b("EB"))
        binary.write(len(content).to_bytes(1, "little"))
    else:
        binary.write(b("E9"))
        binary.write(len(content).to_bytes(4, "little"))
    addr = binary.tell()
    binary.write(content)
    back.compile_num(binary, addr, rel=True)
    
imms["LIT\""] = str_lit

def blob_word(buf, binary, imm, dict, links, back):
    content = bytes.fromhex(word(buf))

    if len(content) < 128:
        binary.write(b("EB"))
        binary.write(len(content).to_bytes(1, "little"))
    else:
        binary.write(b("E9"))
        binary.write(len(content).to_bytes(4, "little"))
    addr = binary.tell()
    binary.write(content)
    back.compile_num(binary, addr, rel=True)
imms["blob"] = blob_word

def bss_alloc_word(buf, binary, imm, dict, links, back):
    content = bytes(int(word(buf)))

    if len(content) < 128:
        binary.write(b("EB"))
        binary.write(len(content).to_bytes(1, "little"))
    else:
        binary.write(b("E9"))
        binary.write(len(content).to_bytes(4, "little"))
    addr = binary.tell()
    binary.write(content)
    back.compile_num(binary, addr, rel=True)
imms["bss-alloc"] = bss_alloc_word

def apply(imm):
    for k, v in imms.items():
        imm[k] = v
