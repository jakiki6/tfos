org 0xa000

%define PAGING_BUFFER 0x4000

stage2:	mov eax, dword [0x7c00 + 12]	; total blocks
	shl eax, 3			; times sizeof(uint64_t)
	add eax, dword [0x7c00 + 28]	; plus block size
	sub eax, 1			; minus 1
	div dword [0x7c00 + 28]		; divided by the block size
	add eax, 16			; allocation table starts at block 16

	mov dword [directory_start], eax

	mov dword [DAP.lba_lower], eax	; read first sector of directory
	mov word [DAP.offset_segment], 0
	mov word [DAP.offset_offset], buffer

	push ax
	mov ah, 0x42
	mov dl, byte [0x7d00]
	mov si, DAP

	mov bp, errors.read_directory
	clc
	int 0x13
	pop ax
	jc error

	mov bx, 0
	mov bp, errors.find_directory

	mov eax, dword [directory_start]
	jmp .proc

.next:	add bx, 256
	cmp bh, 1
	jne .skip
	inc dword [DAP.lba_lower]
	xor bx, bx

.skip:	push ax
	mov ah, 0x42
	mov dl, byte [0x7d00]
	mov si, DAP

	clc
	int 0x13
	pop ax
	jc error

.proc:	cmp dword [bx+buffer], 0	; end of directory?
	je error
					; normal entry?
	cmp dword [bx+buffer], 0xfffffffd
	je .next

					; deleted entry?
	cmp dword [bx+buffer], 0xfffffffe
	je .next

	cmp byte [bx+buffer+8], 0	; file?
	jne .next

	mov si, buffer + 9		; is filename our kernel?
	mov di, kernel_name
	call strcmp
	jc .next

	mov eax, dword [bx+buffer+240]	; read starting block
	mov cx, 0			; pointer to other buffer

.rd:	mov dword [DAP.lba_lower], eax
	mov word [DAP.offset_segment], 0x1000
	mov word [DAP.offset_offset], cx

	push ax
	mov ah, 0x42
	mov dl, byte [0x7d00]
	mov si, DAP

	mov bp, errors.read_block
	clc
	int 0x13
	pop ax
	jc error

	add cx, 512

	mov ebx, eax
	shr eax, 6			; get index in allocation table
	add eax, 16

	mov dword [DAP.lba_lower], eax
	mov word [DAP.offset_segment], 0
	mov word [DAP.offset_offset], buffer
	
	push ax
	mov ah, 0x42
	mov dl, byte [0x7d00]
	mov si, DAP

	mov bp, errors.read_chain
	clc
	int 0x13
	pop ax
	jc error

	and ebx, 63			; mask to index in sector
	shl ebx, 3			; times size of qword
	add ebx, buffer			; get index in buffer

	mov eax, dword [ebx]
	mov ebx, eax

	mov bp, errors.found_reserved_block
	cmp eax, 0xfffffff0
	je error

	cmp eax, 0xffffffff
	je .done

	shr ebx, 6			; read chain table
	add ebx, 16
	mov dword [DAP.lba_lower], ebx
	push ax
	mov ah, 0x42
	mov dl, byte [0x7d00]
	mov si, DAP

	mov bp, errors.read_chain
	clc
	int 0x13
	pop ax
	jc error

	jmp .rd

.done:	push 0x4000
	pop es
	mov di, 0x0000

	call do_e820
	mov bp, errors.e820
	jc error

	push cs
	pop ds
	push cs
	pop es

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

	nop
	nop

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

	nop
	nop

	lgdt [gdt.desc]			; load gdt

	jmp 0x08:long_mode		; JUMP

	align 8
gdt:
.null:	dq 0x0000000000000000		; unused
.code:	dq 0x00209A0000000000		; 64 bit r-x
.data:	dq 0x0000920000000000		; 64 bit rw-
.desc:
	dw gdt.desc - gdt - 1
	dd gdt

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

	mov rsp, 0x210000
	mov rbp, 0x200000

	jmp 0x10000

	bits 16
error:
	mov ah, 0x0e
	mov bx, 0x0007
	mov si, bp

.loop:	lodsb
	cmp al, 0
	je .hlt
	int 0x10
	jmp .loop

.hlt:	cli
	hlt
	jmp .hlt

strcmp:	cmp byte [si], 0
	jne .1
	cmp byte [di], 0
	je .success
	jmp .error
.1:	cmpsb
	je strcmp

.error:	stc
.success:
	ret

print_eax:
	pusha
	mov dword [.eax], eax

	mov cx, 8
.loop:	mov bx, .table

	mov edx, eax
	and eax, 0xf0000000
	shr eax, 28
	xlat
	mov ah, 0x0e
	mov bx, 0x0007
	int 0x10
	xchg edx, eax

	shl eax, 4
	loop .loop

	mov ax, 0x0e0a
	mov bx, 0x0007
	int 0x10
	mov al, 0x0d
	int 0x10

	mov eax, dword [.eax]
	popa
	ret
.eax:	dd 0
.table:	db "0123456789abcdef"
.tmp:	dd 0

do_e820:
	xor ebx, ebx		; ebx must be 0 to start
	mov edx, 0x0534D4150	; Place "SMAP" into edx
	mov eax, 0xe820
	mov [es:di + 20], dword 1	; force a valid ACPI 3.X entry
	mov ecx, 24		; ask for 24 bytes
	int 0x15
	jc short .failed	; carry set on first call means "unsupported function"
	mov edx, 0x0534D4150	; Some BIOSes apparently trash this register?
	cmp eax, edx		; on success, eax must have been reset to "SMAP"
	jne short .failed
	test ebx, ebx		; ebx = 0 implies list is only 1 entry long (worthless)
	je short .failed
	jmp short .jmpin
.e820lp:
	mov eax, 0xe820		; eax, ecx get trashed on every int 0x15 call
	mov [es:di + 20], dword 1	; force a valid ACPI 3.X entry
	mov ecx, 24		; ask for 24 bytes again
	int 0x15
	jc short .e820f		; carry set means "end of list already reached"
	mov edx, 0x0534D4150	; repair potentially trashed register
.jmpin:
	jcxz .skipent		; skip any 0 length entries
	cmp cl, 20		; got a 24 byte ACPI 3.X response?
	jbe short .notext
	test byte [es:di + 20], 1	; if so: is the "ignore this data" bit clear?
	je short .skipent
.notext:
	mov ecx, [es:di + 8]	; get lower uint32_t of memory region length
	or ecx, [es:di + 12]	; "or" it with upper uint32_t to test for zero
	jz .skipent		; if length uint64_t is 0, skip entry
	inc word [.count]	; got a good entry: ++count, move to next storage spot
	add di, 24
.skipent:
	test ebx, ebx		; if ebx resets to 0, list is complete
	jne short .e820lp
.e820f:
	clc			; there is "jc" on end of list to this point, so the carry must be cleared
	ret
.failed:
	stc			; "function unsupported" error exit
	ret
.count:	equ 0xf000

DAP:
.header:
    db 0x10     ; header
.unused:
    db 0x00     ; unused
.count:  
    dw 0x0001   ; number of sectors
.offset_offset:   
    dw buffer   ; offset
.offset_segment:  
    dw 0x0000   ; offset
.lba_lower:
    dd 0	; lba
.lba_upper:
    dd 0	; lba
.end:

buffer:	equ 0xf000

kernel_name:
	db "kernel.bin", 0

directory_start:
	dd 0

errors:
.read_directory:
	db "Cannot read the directory", 0x0a, 0x0d, 0
.find_directory:
	db "Error while finding file in directory", 0x0a, 0x0d, 0
.read_block:
	db "Error while reading a block", 0x0a, 0x0d, 0
.read_chain:
	db "Error while reading a chain entry", 0x0a, 0x0d, 0
.found_reserved_block:
	db "Found reserved block while reading chain", 0x0a, 0x0d, 0
.e820:
	db "E820 not supported :(", 0x0a, 0x0d, 0

idt:	dw 0
	dd 0
