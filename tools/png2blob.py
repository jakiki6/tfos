from PIL import Image
import sys

if len(sys.argv) != 2:
    print(sys.argv[0], "<file>")
    exit(1)

def h(b, g, r):
    return hex(r)[2:].zfill(2) + hex(g)[2:].zfill(2) + hex(b)[2:].zfill(2)

im = Image.open(sys.argv[1]).convert("RGB")

content = ""
content += f"val logo-sx\n  {im.size[0]} to logo-sx\n\n"
content += f"val logo-sy\n  {im.size[1]} to logo-sy\n\n"

content += ": logo-blob\n  blob "
for y in range(0, im.size[1]):
    for x in range(0, im.size[0]):
        content += f"{h(*im.getpixel((x, y)))}"

content += "\n;"

print(content)
