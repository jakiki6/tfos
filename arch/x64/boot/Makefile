all: ../os.img

../os.img: loader.bin
	qemu-img create $@ 64K

	dd if=loader.bin of=$@ conv=notrunc bs=512 count=1

%.bin: %.asm
	nasm -f bin -o $@ $<

clean:
	rm *.o *.bin 2> /dev/null || true

.PHONY: all clean
