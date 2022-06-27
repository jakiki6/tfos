all: ../os.img

../os.img: loader.bin stage2.bin
	qemu-img create $@ 64M

	echfs-utils $@ quick-format 512

	head -c 4 loader.bin | dd of=$@ conv=notrunc
	dd if=loader.bin of=$@ conv=notrunc bs=1 seek=58 skip=58
	dd if=stage2.bin of=$@ conv=notrunc bs=1 seek=512

%.bin: %.asm
	nasm -f bin -o $@ $<

clean:
	rm *.o *.bin 2> /dev/null || true

.PHONY: all clean
