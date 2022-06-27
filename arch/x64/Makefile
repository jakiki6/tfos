all: kernel.bin
	make -C boot
	for file in $^; do \
		echfs-utils os.img import $$file $$file; \
	done

	qemu-system-x86_64 -hda os.img -enable-kvm -m 512M -D log.txt -d cpu_reset,int

clean:
	make -C boot clean

	rm os.img log.txt kernel.bin 2> /dev/null || true

kernel.bin:
	python3 ../../main.py init/main.fs $@

.PHONY: all clean
