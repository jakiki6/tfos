	org 0x7c00
entry:
	cli
	jmp 0x0000:.fix_cs
.fix_cs:
	mov sp, 0x8000

	cli
	mov ah, 0x42
	mov si, DAP
.retry:
	int 0x13
	jc .error

	jmp 0x2000

.error:
	mov ah, 1
	int 0x13
	push 0xb800
    pop ds
    mov byte [0], ah

	cli
.hlt:
	hlt
	jmp .hlt

DAP:
.header:
    db 0x10     ; header
.unused:
    db 0x00     ; unused
.count:  
    dw 0x003   ; number of sectors
.offset_offset:
    dw 0x2000   ; offset
.offset_segment:
    dw 0x0000   ; offset
.lba_lower:   
    dd 1    ; lba
.lba_upper:   
    dd 0    ; lba
.end:

times 510 - ($ - $$) nop
db 0x55, 0xaa
