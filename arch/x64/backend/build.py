import os, subprocess

strings = {}

for root, _, files in os.walk(os.path.join(os.path.dirname(__file__), "asm")):
    for _fn in files:
        if _fn.startswith("$") and len(_fn) > 1:
            name = bytes.fromhex(_fn[1:]).decode()
        else:
            name = _fn

        fn = os.path.join(root, _fn)

        cmd = ["nasm",  "-f", "bin", "-o", "/tmp/build.bin", fn]
        print(cmd)
        subprocess.run(cmd, check=True)

        with open("/tmp/build.bin", "rb") as file:
            strings[name] = file.read()

with open(os.path.join(os.path.dirname(__file__), "strings.txt"), "w") as file:
    for k, v in strings.items():
        file.write(f"{k} {v.hex()}\n")
