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
        numberOfPixelCells qword 0

        pixelReal real8 4 dup (0.0)
        pixelImaginars real8 4 dup (0.0)
        complexPlaneLimit real8 4 dup (4.0)
        iterationResult dword 4 dup (0)


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
            ;idiv rbx, 4h
            sar rbx, 2
            mov r15,4 ;potrzebne do dzialania cmov
            cmp rdx, 0
            cmove rax, r15; jezeli nie zostana cztery komorki tablicy to wtedy do r8 przekazane zostanie tyle ile zostalo

            mov numberOfPixelCells, rax ;zapisanie w ramie ile komorek pamieci bedziemy przetwarzac

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

            ;vcmpeq_ospd = cmpleps CMPPD
            ;vmovmskpd MOVMSKPD
            ;ymm0 - zRealis
            ;ymm1 - zImaginaris
            ;ymm2 - tmp
            ;ymm3 - tmp
            ;pixelReal - px (cRealis) ymm4
            ;pixelImaginars - py - (CImaginaris) ymm5
            ;ymm6 - zliczone iteracje
            ;ymm7 - wektor wypelniony 4, potrzebny do kalkulacji maski
            ;ymm8 - 
            vmovapd ymm4, ymmword ptr [pixelReal]
            vmovapd ymm5, ymmword ptr [pixelImaginars]
            vmovapd ymm7, ymmword ptr [complexPlaneLimit]
            vmovapd ymm0, ymm4 ;ymm0 = zRealis (zaczyna sie od cRealis)
            vmovapd ymm1, ymm5 ;ymm1 = zImaginaris(zaczyna siê od cImaginaris)
            vxorpd  ymm6, ymm6, ymm6; wyzerowanie rejestru z wynikiem
            mov rbx, maxIterator; ustawienie maksymalnej liczby iteracji do licznika petli
            CheckingMandelbrot:
                vmovapd ymm2, ymm0 ;ymm2 = zRealis
                vmulpd  ymm0, ymm0, ymm0 ;ymm0 = zRealis^2
                vmovapd ymm3, ymm1 ;ymm3 = zImaginaris
                vaddpd ymm1, ymm1, ymm1 ;ymm1 = 2*zImaginaris

                vmulpd ymm1, ymm1, ymm2 ;ymm1 = 2*zImaginaris*zRealis
                vmovapd ymm2, ymm0 ;ymm2 = zRealis^2
                vmulpd ymm3, ymm3, ymm3 ;ymm3 = zImaginaris^2

                vmovapd ymm1, ymm5 ;ymm1 = 2*zImaginaris*zRealis*CImaginaris = y
                vsubpd  ymm0, ymm0, ymm3 ;ymm0 = zRealis^2 - zImaginaris^2
                vaddpd  ymm2, ymm2, ymm3 ;ymm2 = zRealis^2 + zImaginaris^2 = warunek while

                vcmplepd ymm2, ymm2, ymm7 ;nadanie ymm2 maski - jezeli ymm2 <= ymm7 (4.0) ustaw jeden. Jezeli przekroczy 4 ustaw 0.
                vaddpd  ymm0, ymm0, ymm4 ;ymm0 = zRealis^2 - zImaginaris^2 + cRealis = x

                vmovmskpd rax, ymm2 ;pobranie maski do rejestr akumulatora
                test rax,rax ;sprawdzenie czy wszystkie sprawdzanie piksele wyszly poza 4.0, jezeli tak przerwij petle
                jz TerminateChecking
                vandpd  ymm2, ymm2, ymm7 ;przepisanie maski do rejestru ymm2, maski uzywamy do inkrementowania rejestru wynikowego ymm6
                vaddpd ymm6, ymm6, ymm2 ;dodanie rejestru ymm2 przechowoujacego wyniki iteracji petli do rejestru przechowujacego wynik
                sub rbx,1
                jnz CheckingMandelbrot
          TerminateChecking:
            ;przepisanie wyniku dzialania petli z rejestru ymm6 do kolejnych komorek tablicy wynikowej z funkcja koloryzujaca
            ;vmovapd ymmword ptr [iterationResult], ymm6

            ;ymm0 - r
            vmovapd ymm7, ymmword ptr [complexPlaneLimit]
            vdivpd  ymm6, ymm6, ymm7 ;podzielenie wyniku przez 4, dzieki czemu otrzymamy sama liczbe iteracji 
            vcvtpd2udq xmm0, ymm6
            movdqu  xmmword ptr [iterationResult], xmm0 ;cztery wartosci pikseli

            mov rbx, numberOfPixelCells; liczba pikseli do zapisu!! - tutaj ustawic potem poprawke na tablice nie podzielna przez 4
            mov r8d, [iterationResult]
            mov r9, [imageBuffer]
            xor r10d, r10d
            mov r14d,256
            ;rdx, rax, rcx
            WriteLoop:
                mov r11d, dword ptr [r8 + r10]; r
                mov r12d,r11d; g
                mov r13d, r11d; b
                ;int r = ((int)(n * 2 * n) % 256)
                imul r11d,r11d
                add r11d,r11d
                mov eax, r11d
                idiv r14d
                mov r11d, edx
                ;int g = ((n * n) % 256)
                imul r12d,r12d
                mov eax, r12d
                idiv r14d
                mov r12d,edx
                ;int b = (n % 256)
                mov eax, r13d
                idiv r14d
                mov r13d, edx

                add r10d, 4
                dec rbx

                ;obliczanie indeksu 3*(i-subTabBeginPoint)
                mov rax, rcx
                mov r15, subTabBeginPoint
                sub rax, r15
                imul rax, 3
                add rax, r9 ;dodanie do indeksacji tablicy jej polozenia w pamieci zdefiniowanego w rejestrze r9
                ;imageBuffer[3 * (i-subTabBeginPoint)] = b;
                mov dword ptr[rax + r10], r11d
			    ;imageBuffer[(3 * (i-subTabBeginPoint)) + 1] = g;
                mov dword ptr[rax + r10], r12d
			    ;imageBuffer[(3 * (i-subTabBeginPoint)) + 2] = r;
                mov dword ptr[rax + r10], r13d
                jnz WriteLoop

        add rcx, 4; i+= 4
        ;sprawdzenie warunku petli
        cmp rcx,  subArrayOffset
        jl ComplexPlaneLoop
        
EndOfLoop:
       ret

generateMandelbrotFraktalAsm endp
end
;-------------------------------------------------------------------------
