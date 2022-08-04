panic:
	lodsb
	cmp al, 0
	je .hlt

	mov ah, 0x0e
	mov bx, 0x0007
	int 0x10
	jmp panic

.hlt:
	cli
	hlt
	jmp .hlt

errors:
.no_vesa:
	db "Your BIOS doesn't support VESA!", 0
.no_e820:
	db "Your BIOS doesn't provide a memory map!", 0
.no_mode:
	db "No adequate VESA mode found!", 0

print_hex:
	mov bx, .tbl

	mov ax, si
	shr ax, 12
	xlat
	call _print

	mov ax, si
	shr ax, 8
	and al, 0x0f
	xlat
	call _print

	mov ax, si
	shr ax, 4
	and al, 0x0f
	xlat
	call _print

	mov ax, si
	and al, 0x0f
	xlat
	call _print

	ret

.tbl:
	db "0123456789abcdef"

print_spc:
	mov al, 0x20
	call _print
	ret

print_nl:
	mov al, 0x0a
	call _print
	mov al, 0x0d
	call _print
	ret


_print:
	pusha
	mov ah, 0x0e
	mov bx, 0x0007
	int 0x10
	popa
	ret
