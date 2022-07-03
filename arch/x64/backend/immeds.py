from backends import b, word, backends

imms = {}

stack = []

def define_word(buf, binary, imm, dict, back):
    binary.write(b("E9"))
    stack.append(binary.tell())
    binary.write(b("00000000"))
    dict[word(buf)] = binary.tell()
imms[":"] = define_word

def end_word(buf, binary, imm, dict, back):
    binary.write(b("C3"))
    target = stack.pop()
    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.seek(p)
imms[";"] = end_word

def cond_clause(buf, binary, imm, dict, back):
    binary.write(b("4883ED0848837D00000F84"))
    stack.append(binary.tell())
    binary.write(b("00000000"))
imms["if"] = cond_clause

def then_clause(buf, binary, imm, dict, back):
    target = stack.pop()
    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True))
    binary.seek(p)
imms["then"] = then_clause

def else_clause(buf, binary, imm, dict, back):
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

def begin_clause(buf, binary, imm, dict, back):
    stack.append(binary.tell())
imms["begin"] = begin_clause

def again_clause(buf, binary, imm, dict, back):
    target = stack.pop()
    rel = target - binary.tell() - 5
    binary.write(b("E9"))
    binary.write(rel.to_bytes(4, "little", signed=True))
imms["again"] = again_clause

def until_clause(buf, binary, imm, dict, back):
    target = stack.pop()
    rel = target - binary.tell() - 15
    binary.write(b("4883ED0848837D00000F84"))
    binary.write(rel.to_bytes(4, "little", signed=True))
imms["until"] = until_clause

def val_word(buf, binary, imm, dict, back):
    binary.write(b("EB11"))
    dict[word(buf)] = binary.tell()
    binary.write(b("48B8"))
    binary.write(bytes(8))
    binary.write(b("488945004883C508C3"))
imms["val"] = val_word

def to_word(buf, binary, imm, dict, back):
    back.compile_num(binary, dict[word(buf)] + 2)
    back.compile_ref(binary, dict["!"])
imms["to"] = to_word

def hyphen_word(buf, binary, imm, dict, back):
    back.compile_num(binary, dict[word(buf)])
imms["'"] = hyphen_word

def str_lit(buf, binary, imm, dict, back):
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
        else:
            content += c

    content = content.encode("utf-8") + b"\x00"

    if len(content) < 128:
        binary.write(b("EB"))
        binary.write(len(content).to_bytes(1, "little"))
    else:
        binary.write(b("E9"))
        binary.write(len(content).to_bytes(4, "little"))
    addr = binary.tell()
    binary.write(content)
    back.compile_num(binary, addr)
imms["LIT\""] = str_lit

def comment(buf, binary, imm, dict, back):
    while word(buf) != ")":
        continue
imms["("] = comment

def apply(imm):
    for k, v in imms.items():
        imm[k] = v
