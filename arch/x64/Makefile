all: kernel.bin
	make -C boot
	dd if=$< of=os.img bs=512 seek=4 conv=notrunc

	qemu-system-x86_64 -hda os.img -m 512M -D log.txt -d cpu_reset,int -machine smm=off -serial stdio -no-reboot --enable-kvm

clean:
	make -C boot clean

	rm os.img log.txt kernel.bin 2> /dev/null || true

kernel.bin:
	python3 ../../main.py arch/x64/init/main.fs $@

.PHONY: all clean
