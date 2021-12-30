;-------------------------------------------------------------------------
  includelib      msvcrtd
        .data
        
        imageBuffer byte 0 ;wskaznik na tablice byte[] 
        ;obecnie tablica w RCX
        subTableBeginPoint qword 0
        sizeOfSubTable qword 0
        dimensionX dword 0
        dimensionY dword 0 ;int - dd
        maxIterator qword   0; long - qword
        minR real8 0.0;double - real8
        maxR real8 0.0;double
        minI real8 0.0;double
        maxI real8 0.0;double
        .code
        public  DllMain
        public  generateMandelbrotFraktalAsm

DllMain proc                            ;return true
        mov     rax, 1
        ret     0
DllMain endp


generateMandelbrotFraktalAsm proc
        mov     rax, 11111111
        mov     [rcx],rax
       ;byte[] imageBuffer - rcx
       ;long subTableBeginPoint - rdx
       ;long sizeOfSubTable - r8
       ;int dimensionX - r9
       ;int dimensionY - stack
       ;long maxIterator - stack
       ;double minR - stack
       ;double maxR - stack
       ;double minI - stack
       ;double maxI - stack

       ;mov imageBuffer, [rcx]

       mov subTableBeginPoint, rdx

       mov sizeOfSubTable, r8

       mov dimensionX, r9d

       mov eax, dword ptr [rsp+40]
       mov dimensionY, eax

       mov rax, qword ptr [rsp+48]
       mov maxIterator, rax

       mov rax, real8 ptr [rsp+56]
       mov minR, rax

       mov rax, real8 ptr [rsp+64]
       mov maxR, rax

       mov rax, real8 ptr [rsp+72]
       mov minI, rax

       mov rax, real8 ptr [rsp+80]
       mov maxI, rax

       ret
generateMandelbrotFraktalAsm endp
end
;-------------------------------------------------------------------------
