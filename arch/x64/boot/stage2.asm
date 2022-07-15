org 0x2000

%define PAGING_BUFFER 0x8000

entry:
	push cs
	pop ss
	mov sp, 0xffff
	push cs
	pop ds
	push cs
	pop es
	
	clc
	mov ah, 0x42
	mov si, DAP
.retry:	int 0x13
	jc .retry

jump:
	in al, 0x92			; enable A20 line
	or al, 0x02
	out 0x92, al

	mov di, PAGING_BUFFER

	push di
	mov ecx, 0x1000
	xor eax, eax
	cld
	rep stosd
	pop di

	lea eax, [es:di + 0x1000]
	or eax, 0b11
	mov dword [es:di], eax

	lea eax, [es:di + 0x2000]
	or eax, 0b11
	mov dword [es:di + 0x1000], eax

	push di

	lea di, [di + 0x2000]
	mov eax, 0b10000011

.loop_pt:
	mov [es:di], eax
	add eax, 0x1000
	add di, 8
	cmp eax, 0x200000
	jb .loop_pt

	pop di

	cli
	mov al, 0xff			; disable all irqs
	out 0xa1, al
	out 0x21, al

	lidt [idt]

	mov eax, 0b10100000		; set pae and pge bit
	mov cr4, eax

	mov edx, PAGING_BUFFER		; point to pml4
	mov cr3, edx

	mov ecx, 0xc0000080		; read from efer
	rdmsr

	or eax, 0x00000100		; set lme bit
	wrmsr

	mov ebx, cr0			; activate long mode
	or ebx, 0x80000001		; enable paging and protection
	mov cr0, ebx

	lgdt [gdt.desc]			; load gdt

	jmp 0x08:long_mode	; JUMP

	align 8
gdt:
.null:	dq 0x0000000000000000		; unused
.code:	dq 0x00209A0000000000		; 64 bit r-x
.data:	dq 0x0000920000000000		; 64 bit rw-
.desc:
	dw gdt.desc - gdt - 1
	dq gdt

	bits 64
long_mode:
	mov ax, 0x10
	mov ds, ax
	mov es, ax
	mov fs, ax
	mov gs, ax
	mov ss, ax

	mov rdi, 0xb8000
	push rdi
	mov rcx, 500
	mov rax, 0x1f201f201f201f20
	rep stosq
	pop rdi

	mov dx, 0x03d4
	mov al, 0x0a                    ; cursor shape register
	out dx, al

	inc dx
	mov al, 0x20                    ; bit 5 -> disable cursor
	out dx, al

	mov rsp, 0x210000
	mov rbp, 0x200000

	jmp 0x10000

	bits 16

DAP:
.header:
    db 0x10     ; header
.unused:
    db 0x00     ; unused
.count:  
    dw 0x007f   ; number of sectors
.offset_offset:   
    dw 0x0000   ; offset
.offset_segment:  
    dw 0x1000   ; offset
.lba_lower:
    dd 4	; lba
.lba_upper:
    dd 0	; lba
.end:

idt:	dw 0
	dd 0
