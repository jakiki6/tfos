all: boot
	qemu-system-x86_64 -hda os.img -m 512M -D log.txt -d cpu_reset,int -machine smm=off -serial stdio -no-reboot -bios /usr/share/ovmf/OVMF.fd --enable-kvm

clean:
	rm -f os.img log.txt kernel.efi 2> /dev/null || true

boot: kernel.efi
	make -C boot

kernel.efi:
	python3 ../../main.py arch/x64/init/main.fs $@

.PHONY: all clean boot
