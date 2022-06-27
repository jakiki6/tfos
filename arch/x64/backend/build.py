import os

strings = {}

for root, _, files in os.walk(os.path.join(os.path.dirname(__file__), "asm")):
    for _fn in files:
        fn = os.path.join(root, _fn)

        cmd = f"nasm -f bin -o /tmp/build.bin {fn}"
        print(cmd)
        if os.system(cmd):
            exit(1)

        with open("/tmp/build.bin", "rb") as file:
            strings[_fn] = file.read()

with open(os.path.join(os.path.dirname(__file__), "strings.txt"), "w") as file:
    for k, v in strings.items():
        file.write(f"{k} {v.hex()}\n")
