;-------------------------------------------------------------------------
  includelib      msvcrtd
        ;includelib      oldnames        ;optional
        .data
        .data?
        .code
        public  DllMain
        public  MyProc1

DllMain proc                            ;return true
        mov     rax, 1
        ret     0
DllMain endp

MyProc1 proc                            ;[rcx] = 0123456789abcdefh
        mov     rax, 0123456789abcdefh
        mov     [rcx],rax
        ret     0
MyProc1 endp
end
;-------------------------------------------------------------------------
