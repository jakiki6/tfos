vesa_setup:
    mov ax, 0x4f00
    mov di, vbe_info
    int 0x10
    jc $

    cmp ax, 0x4f 
    jne $

    cmp dword [vbe_info.signature], 'VESA' 
    jne $
    
    mov ax, word [vbe_info.video_mode_seg]
    mov es, ax
    mov di, word [vbe_info.video_mode_offset]
.loop:
    cmp word [es:di], 0xffff
    je $

    push es
    push di

    mov ax, 0x4f01
    mov cx, word [es:di]

    push 0
    pop es

    mov di, vbe_mode_info
    int 0x10

    pop di
    pop es

    cmp byte [vbe_mode_info.bpp], 24
    jne .back

    cmp word [vbe_mode_info.width], 1024
    jne .back

    cmp word [vbe_mode_info.height], 768
    jne .back

    mov ax, 0x4f02
    mov bx, word [es:di]
    int 0x10

    ret
.back:
    add di, 2
    jmp .loop

vbe_mode_info:
times 16 db 0
.pitch: dw 0
.width: dw 0
.height: dw 0
times 3 db 0
.bpp: db 0
times 14 db 0
.framebuffer: dd 0
times 212 db 0

vbe_info:
.signature: dd 0
.version_minor: db 0
.version_magor: db 0
.oem_offset: dw 0
.oem_seg: dw 0
.capabilities: dd 0
.video_mode_offset: dw 0
.video_mode_seg: dw 0
.video_mem_blocks: dw 0
.software_rev: dw 0
.vendor_off: dw 0
.vendor_seg: dw 0
.product_name_off: dw 0
.product_name_seg: dw 0
.project_rev_off: dw 0
.project_rev_seg: dw 0
times 222 db 0
.oem_data times 256 db 0
