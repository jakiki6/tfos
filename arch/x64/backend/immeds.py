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
    binary.write(rel.to_bytes(4, "little", signed=True));
    binary.seek(p)
imms[";"] = end_word

def cond_clause(buf, binary, imm, dict, back):
    binary.write(b("4883ED0848837D00000F84"))
    stack.append(binary.tell())
    binary.write(b("00000000"))
imms["IF"] = cond_clause

def then_clause(buf, binary, imm, dict, back):
    target = stack.pop()
    rel = binary.tell() - target - 4
    p = binary.tell()
    binary.seek(target)
    binary.write(rel.to_bytes(4, "little", signed=True));
    binary.seek(p)
imms["THEN"] = then_clause

def comment(buf, binary, imm, dict, back):
    while word(buf) != ")":
        continue
imms["("] = comment

def apply(imm):
    for k, v in imms.items():
        imm[k] = v
