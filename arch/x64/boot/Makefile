all: ../os.img

../os.img: loader.bin
	dd if=/dev/urandom of=$@ bs=1M count=4

	dd if=loader.bin of=$@ conv=notrunc bs=512 count=4

loader.bin: bootsec.bin stage2.bin
	cat $^ > $@

%.bin: %.asm
	nasm -f bin -o $@ $<

clean:
	rm *.o *.bin 2> /dev/null || true

.PHONY: all clean
