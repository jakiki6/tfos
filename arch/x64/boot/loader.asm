	org 0x7c00
	bits 16

jmp stage1

times 58 db 0x69

stage1:
	cli
	xor ax, ax
	xor bx, bx
	xor cx, cx
	mov byte [drive], dl
	xor dx, dx
	xor si, si
	xor di, di

	push 0x2000
	pop ss
	mov esp, 0xffff
	
	push cs
	pop ds
	push cs
	pop es

	sti

.load_all:
	mov ah, 0x42
	mov dl, byte [drive]
	mov si, DAP

	clc
	int 0x13
	jc .load_all


	xor ax, ax
	xor bx, bx
	xor cx, cx
	xor dx, dx
	xor si, si
	xor di, di
	xor bp, bp

	jmp 0xa000

times 256 - ($ - $$) nop

drive:	db 0

DAP:
.header:
    db 0x10	; header
.unused:
    db 0x00     ; unused
.count:  
    dw 0x000f   ; number of sectors
.offset_offset:
    dw 0xa000	; offset
.offset_segment:
    dw 0x0000   ; offset
.lba_lower:
    dq 1	; lba
.lba_upper:
    dq 0	; lba
.end:

times 510 - ($ - $$) nop
dw 0xaa55
