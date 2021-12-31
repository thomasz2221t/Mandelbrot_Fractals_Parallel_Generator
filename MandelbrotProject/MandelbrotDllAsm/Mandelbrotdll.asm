;-------------------------------------------------------------------------
  includelib      msvcrtd
        .data
        
        imageBuffer qword 0 ;wskaznik na tablice byte[] 
        ;obecnie tablica w RCX
        subTabBeginPoint qword 0
        sizeOfSubTable qword 0
        dimensionX dword 0
        dimensionY dword 0 ;int - dd
        maxIterator qword   0; long - qword
        minR real8 0.0;double - real8
        maxR real8 0.0;double
        minI real8 0.0;double
        maxI real8 0.0;double
        subArrayOffset qword 0
        pixelX dword 0
        pixelY dword 0

        pixelReal real8 4 dup (0.0)
        pixelImaginars real8 4 dup (0.0)


        .code
        public  DllMain
        public  generateMandelbrotFraktalAsm

DllMain proc                            ;return true
        mov     rax, 1
        ret     0
DllMain endp


generateMandelbrotFraktalAsm proc
        ;mov     rax, 11111111
        ;mov     [rcx],rax

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

       mov imageBuffer, rcx

       mov subTabBeginPoint, rdx

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

       ;for (long i = subTabBeginPoint; i < (sizeOfSubTable+subTabBeginPoint); i++)
       mov rcx, subTabBeginPoint ;i = subTabBeginPoint
       ; sizeofSubTable + subTabBeginPoint
       mov rbx, sizeOfSubTable
       add rbx, rcx
       mov subArrayOffset, rbx
       ;sprawdzenie warunku petli
       cmp rcx,  subArrayOffset
       jge EndOfLoop
       ;rcx- iterator
ComplexPlaneLoop:
            ;TO DO zrobic sprawdzenie czy tablica podzielna na 4, jak nie to zawezic zakres iterowania w drugiej petli
            ;if(sizeOfSubTable + subTabBeginPoint) - i >4)
            ;{
            ;    mov r8,4
            ;}else{
            ;   mov r8, (sizeOfSubTable + subTabBeginPoint) - i
            ;}
            ;albo
            ;   je¿eli  if(sizeOfSubTable + subTabBeginPoint) - i >4) to rejestr sumy = 4
            ;   mov r8, rejestr z sizeOfSubTable + subTabBeginPoint - i
            mov rbx, sizeOfSubTable
            add rbx, subTabBeginPoint
            sub rbx, rcx
            mov rax, rbx ;buffor zeby zapamietac wartosc operacji
            div rbx, 4
            cmp rdx, 0
            cmove rax, 4; jezeli nie zostana cztery komorki tablicy to wtedy do r8 przekazane zostanie tyle ile zostalo
            
            mov r8, rax ;przygotowanie petli sterujacej uzyskiwaniem 4 elementowego wektora doubli dla cReal i cImaginaris
            mov r9, [pixelReal]
            mov r10, [pixelImaginars]
            xor rbx, rbx ;table offset
			GettingVectorsLoop:
            ;r8,r9,r10,rbx zajete
                mov rax, rcx; rax = i
                add rax, r8; i+aktualna iteracja
                dec rax; uzyskanie komorki wektora od konca
                mov r11d, dimensionX; r11 = dimensionX
                div r11; i / dimensionX, RAX - wynik dzielenia, reszta z dzielenia - RDX
                ;int y = i / dimensionX;
                mov pixelY, eax
                ;int x = i % dimensionX - rdx
                mov pixelX, edx
	            ;double cReal =  x * ((maxR - minR) / dimensionX) + minR;
                mov rax, maxR
                mov r12, minR
                sub rax, r12 ;maxR - minR
                div r11; (maxR - minR)/ dimensionX
                mov edx, pixelX
                mul rdx ;x * ((maxR - minR) / dimensionX)
                add rax, r12 ;x * ((maxR - minR) / dimensionX) + minR

                ;przepisanie do tabeli pixelReal
                mov rbx, r8
                dec rbx
                mov real8 ptr [r9 + rbx*4], rax

	            ; cImaginaris =  y * ((maxI - minI) / dimensionY) + minI;
                mov rax, maxI
                mov r12, minI
                sub rax, r12 ;maxI - minI
                mov r11d, dimensionY
                div r11 ; (maxI - minI) / dimensionY)
                mov edx, pixelY
                mul rdx ;y * ((maxI - minI) / dimensionY)
                add rax, r12 ;y * ((maxI - minI) / dimensionY) + minI;

                ;przepisanie do tabeli pixelImaginaris
                mov real8 ptr [r10 + rbx*4], rax
                
                dec r8
                jnz GettingVectorsLoop

            ;int i = 0;
	        ;double zRealis = 0.0,
            ;double zImaginaris = 0.0;
	        ;while ((i < max_iteracji) && (zRealis * zRealis + zImaginaris * zImaginaris <= 4.0))

	        ;{
		    ;    double bufor = zRealis * zRealis - zImaginaris * zImaginaris + cRealis;
		    ;    zImaginaris = 2.0 * zRealis * zImaginaris + cImaginaris;
		    ;    zRealis = bufor;
		    ;    i++;
	        ;}

            ;int n = i

			;int r = ((int)(n * 2 * n) % 256)
			;int g = ((n * n) % 256)
			;int b = (n % 256)

			;imageBuffer[3 * (i-subTabBeginPoint)] = b;
			;imageBuffer[(3 * (i-subTabBeginPoint)) + 1] = g;
			;imageBuffer[(3 * (i-subTabBeginPoint)) + 2] = r;

		;}

        add rcx, 4; i+= 4
        ;sprawdzenie warunku petli
        cmp rcx,  subArrayOffset
        jl ComplexPlaneLoop
        
EndOfLoop:
       ret

generateMandelbrotFraktalAsm endp
end
;-------------------------------------------------------------------------
