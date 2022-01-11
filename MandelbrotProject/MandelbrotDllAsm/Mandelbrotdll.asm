;--------------------;-------------------------------------------------------------------------
  includelib      msvcrtd
        ;.data
        .const
        maskConst real8 4.0
        
        ;imageBuffer byte 0 ;wskaznik na tablice byte[] 
        ;obecnie tablica w RCX
        ;subTabBeginPoint qword 0
        ;sizeOfSubTable qword 0
        ;dimensionX dword 0
        ;dimensionY dword 0 ;int - dd
        ;maxIterator qword   0; long - qword
        ;minR real8 0.0;double - real8
        ;maxR real8 0.0;double
        ;minI real8 0.0;double
       ; maxI real8 0.0;double
        ;subArrayOffset qword 0
        ;pixelX dword 0
        ;pixelY dword 0
        ;numberOfPixelCells qword 0

        ;pixelReal real8 4 dup (0.0)
       ; pixelImaginars real8 4 dup (0.0)
        ;complexPlaneLimit real8 4 dup (4.0)
        ;iterationResult dword 4 dup (0)


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

       ;rbp register - rcx qword ptr [rbp-40]
       ;byte[] imageBuffer - rcx byte ptr (ptrXD)[rbp-8]
       ;long subTableBeginPoint - rdx qword ptr [rbp-16]
       ;long sizeOfSubTable - r8 qword ptr [rbp-24]
       ;int dimensionX - r9 dwrod ptr [rbp-32]
       ;int dimensionY - stack dword ptr [rbp+48]40
       ;long maxIterator - stack qword ptr [rbp+56]48
       ;double minR - stack real8 ptr [rbp+64]56
       ;double maxR - stack real8 ptr [rbp+72]64
       ;double minI - stack real8 ptr [rbp+80]72
       ;double maxI - stack real 8ptr [rbp+88]80

       ;double[] imageBuffer - rcx real8 ptr [+8
       ;double[] tableMappedToReal - rdx real8 +16 
       ;double[] tableMappedToImaginaris - r8 real8 +24
       ;long subTabBeginPoint - r9 qword +32
       ;long sizeOfSubTable - stack dword ptr [rbp+40]
       ;long maxIteration - stack qword ptr [rbp+48]

       push rbp ;Wrzucenie rbp na baze zeby miec odnosnik do pracy ze stosem
       mov rbp, rsp ;przepisanie wartosci rsp (aktualnie wskazywana komorka stosu) do rejestru rbp
       push rdi
       push rsi
       push rbx
       push r10
       push r11
       push r12
       push r13
       push r14
       push r15
       ;sub rsp, 40 ;zaalokowanie miejsca na stosie na 4 zmienne dynamiczne
       ;push rcx ;zaladowanie na stos wskaznika na tablice
       ;push rdx ;zaladowanie na stos subTabBeginPoint
       ;push r8 ;zaladowanie na stos sizeOfSubTable
       ;push r9 ;zaladowanie na stos dimensionX
       ;push rbx ;zaladowanie rejestru bazy na stos
       ;//=============================================================
       ;rejrestry zajete: rbp,rcx - i
       ;//================================================================

       ;for (long i = subTabBeginPoint; i < (sizeOfSubTable+subTabBeginPoint); i++)
       ;g³ówna petla
       xor r15,r15
       mov rsi, r9 ;i = subTabBeginPoint
       ;xor rbx,rbx
       mov rbx, qword ptr [rbp+48]
       ; sizeofSubTable + subTabBeginPoint
       add rbx, rsi
       ;sprawdzenie warunku petli
       cmp rsi, rbx
       jge EndOfLoop
       ;r10- iterator
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

            ;mov r8, 4 ;przygotowanie petli sterujacej uzyskiwaniem 4 elementowego wektora doubli dla cReal i cImaginaris
            ;xor rbx, rbx ;table offset
            ;mov r10, rsp; w r10 wskaznik na gore stosu zmiennych y
            ;sub rsp,32 ;przygotowanie miejsca na 4 zmiennych lokalne
            ;mov r11, rsp; w r11 wskaznik na gore stosu zmiennych x
            ;sub rsp,32
            ;mov r9d, dword ptr [rbp-32]; w r9 dimensionX
			;GettingVectorsLoop:
            ;;r8,rbx,rcx,rbp zajete
            ;   mov rax, rcx; rax = i
            ;   add rax, rbx; i+0, i+1, i+2, i+3
            ;   ;mov r9d, dword ptr [rbp-32]; w r9 dimensionX
            ;   idiv r9; i / dimensionX, RAX - wynik dzielenia, reszta z dzielenia - RDX
            ;   ;int y = i / dimensionX
            ;
            ;   ;inc rbx
            ;   mov [r10 + rbx*8], rax ;[rbp-48], [rbp-56], [rbp-64], [rbp-72]
            ;   ;int x = i % dimensionX - rdx
            ;   mov [r11 + rbx*8], rdx ;[rbp-80], [rbp-88], [rbp-96], [rbp- 104]
            ;
            ;  inc rbx
            ;   dec r8
            ;   jnz GettingVectorsLoop
            ;
            ;vmovdqu ymm4, ymmword ptr [rsp-32] ;przepisanie wartosci x do rejestru ymm2
            ;vmovdqu ymm3, ymmword ptr [rsp-64] ;przepsianie wartosci y do rejestru ymm1
            ;;add rsp,64; zwolnienie zmiennych lokalnych x i y ze stosu 
            ;
            ;;vcvtqq2pd ymm1, ymm3 ;ymm1 - y
            ;;vcvtqq2pd ymm2, ymm4 ;ymm2 - x


            ;;przepisanie wartosc iteratora intowych do wektora SSE
            ;;mov r8, rcx; przepisanie wartoœci i do eax
            ;;mov r9, r8
            ;;inc r9 ;wartosc i+1 w r9
            ;;mov r10, r9
            ;;inc r10 ;wartosc i+2 w r10
            ;;mov r11, r10
            ;;inc r11 ;wartosc i+3 w r11
            ;;sub rsp,24 ;przygotowanie miejsca na 4 zmienne lokalne
            ;;push r8 ;rcx qword ptr [rbp-48]
            ;;push r9 ;rcx qword ptr [rbp-56]
            ;;push r10 ;rcx qword ptr [rbp-64]
            ;;push r11 ;rcx qword ptr [rbp-72]
            ;;vmovdqa ymm1, ymmword ptr [rsp] ;przepisanie wartosci longowych do ymm0
            ;;add rsp,24; zwolnienie zaalokowanego stosu

            
            ;;w xmm1 - y, xmm2 - x, xmm3 - dimensionX zbroadcastowane
            ;;vmulps
            ;;vmovapd  ymm2, ymm1 ;w xmm2 wartosci i,i+1,i+2,i+3
            ;;movd xmm3, qword ptr [rbp-32] ;w xmm3 wartosci dimensionX, dimensionX
            ;;vpbroadcastq ymm3, xmm3 ;w ymm3 dimensionX, dimensionX, dimensionX, dimensionX
            ;;int y = i / dimensionX;
            ;;vdivsd ymm1, ymm1, ymm3 ;uzyskiwanie wartosci y dla piksela dzielac i/dimensionX
            ;;int x = i % dimensionX 
            ;;vmovapd ymm4, ymm4, ymm1
            ;;vmulsd ymm4, ymm4, ymm3
            ;;vsubsd ymm2, ymm2, ymm3 ;uzyskanie reszty z dzielenia i/dimensionX, czyli wartosc x


            ;;ymm1 - y, ymm2 - x, ymm3 - dimensionX

            ;;double range = maxR - minR;
            ;;mov rax, real8 ptr [rbp+72]; rax = maxR
            ;;mov rdx, real8 ptr [rbp+64]; rdx = minR
            ;;sub rax, rdx ;maxR - minR
            ;;mov r9d, dword ptr [rbp-32]; w r9 dimensionX
            ;;idiv r9 ;eax wynik dzielenia
            ;;movd  xmm0, rax
            ;;vbroadcastsd ymm4, xmm0; w ymm4 ((maxR - minR) / dimensionX)
           ;; vmulpd ymm4, ymm4,ymm2 ;x * ((maxR - minR) / dimensionX) + minR; = ymm4
           ;; movd xmm1, rdx
           ;; vbroadcastsd ymm3, xmm1
           ; ;double cReal =  x * ((maxR - minR) / dimensionX) + minR;
           ;; vaddpd ymm4, ymm4, ymm3 ;ymm4 = cReal = x * ((maxR - minR) / dimensionX) + minR

            ;;double range = maxI - minI;
            ;mov rax, real8 ptr [rbp+88]; rax = maxI
            ;mov rdx, real8 ptr [rbp+80]; rdx = minI
            ;sub rax, rdx ;maxI - minI
            ;mov r9d, dword ptr [rbp+48]; r9 = dimensionY
            ;idiv r9 ;eax wynik dzielenia
            ;movd xmm0, rax
            ;vbroadcastsd ymm5, xmm0; w ymm5 ((maxI - minI) / dimensionY)
            ;vmulpd ymm5, ymm5, ymm1 ;y * ((maxI - minI) / dimensionY)
            ;movd xmm1, rdx
            ;vbroadcastsd ymm3, xmm1 ;wartosci minI w calym ymm3
            ;; cImaginaris =  y * ((maxI - minI) / dimensionY) + minI;
            ;vaddpd ymm5, ymm5, ymm3; ymm5 = cImaginaris =  y * ((maxI - minI) / dimensionY) + minI 

            ;;int i = 0;
	        ;;double zRealis = 0.0,
            ;;double zImaginaris = 0.0;
	        ;;while ((i < max_iteracji) && (zRealis * zRealis + zImaginaris * zImaginaris <= 4.0))

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

            mov r11, rsi ;przeslanie iteratora do rax
            sub r11, r9 ;i - subTabBeginPoint
            vmovupd ymm4, ymmword ptr [rdx][r11*8] ; pobranie cReal do ymm4 ;0,32,64,128
            vmovupd ymm5, ymmword ptr [r8][r11*8] ; pobranie cImaginaris do ymm5 ;0,64
            ;vmovapd ymm7, ymmword ptr [complexPlaneLimit]
            mov rax, maskConst
            movd xmm0,rax
            vbroadcastsd ymm7, xmm0 ;wypelnienie ymm7 wartosciami 4.0
            vmovupd ymm0, ymm4 ;ymm0 = zRealis (zaczyna sie od cRealis)
            vmovupd ymm1, ymm5 ;ymm1 = zImaginaris(zaczyna siê od cImaginaris)
            vxorpd  ymm6, ymm6, ymm6; wyzerowanie rejestru z wynikiem
            mov rbx, qword ptr [rbp+56]; ustawienie maksymalnej liczby iteracji do licznika petli
            CheckingMandelbrot:
                vmovupd ymm2, ymm0 ;ymm2 = zRealis
                vmulpd  ymm0, ymm0, ymm0 ;ymm0 = zRealis^2
                vmovupd ymm3, ymm1 ;ymm3 = zImaginaris
                vaddpd ymm1, ymm1, ymm1 ;ymm1 = 2*zImaginaris

                vmulpd ymm1, ymm1, ymm2 ;ymm1 = 2*zImaginaris*zRealis
                vmovupd ymm2, ymm0 ;ymm2 = zRealis^2
                vmulpd ymm3, ymm3, ymm3 ;ymm3 = zImaginaris^2

                vaddpd ymm1, ymm1, ymm5 ;ymm1 = 2*zImaginaris*zRealis+CImaginaris = y
                vsubpd  ymm0, ymm0, ymm3 ;ymm0 = zRealis^2 - zImaginaris^2
                vaddpd  ymm2, ymm2, ymm3 ;ymm2 = zRealis^2 + zImaginaris^2 = warunek while

                vcmplepd  ymm2, ymm2, ymm7 ;nadanie ymm2 maski - jezeli ymm2 <= ymm7 (4.0) ustaw jeden. Jezeli przekroczy 4 ustaw 0.
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
            ;mov rax, maskConst
            ;movd xmm1,rax
            ;vbroadcastsd ymm7, xmm0 ;wypelnienie ymm7 wartosciami 4.0
            ;vdivpd  ymm6, ymm6, ymm7 ;podzielenie wyniku przez 4, dzieki czemu otrzymamy sama liczbe iteracji 
            vmovupd ymmword ptr [rcx][r11*8], ymm6
            ;================== wroc tu 
            ;vcvtpd2dq xmm0, ymm6; konwersja wyniku na integery
            ;movdqu  xmmword ptr [iterationResult], xmm0 ;cztery wartosci pikseli
            ;sub rsp, 32
            ;movdqa xmmword ptr [rsp+32], xmm0


            ;mov rbx, 4; liczba pikseli do zapisu!! - tutaj ustawic potem poprawke na tablice nie podzielna przez 4
            ;mov r8d, [iterationResult]
            ;mov r9, [imageBuffer]
            ;mov r10b, byte ptr [rbp-8]
            ;xor r8, r8
            ;WriteLoop:
                ;mov edx, dword ptr [rsp + r8]; r
                ;mov r9,r11d; g
                ;mov r13d, r11d; b
                ;int r = ((int)(n * 2 * n) % 256)
                ;imul edx,edx
                ;add edx,edx
                ;mov eax, edx
                ;mov r9, 256
                ;idiv r9
                ;mov rax, rcx
                ;mov r11, qword ptr [rbp-16]; r11 = subTabBeginPoint
                ;sub rax, r11; i-subTabBeginPoint
                ; imul rax, 3 ;3 * (i-subTabBeginPoint))
                ;mov byte ptr[r10 + rax], dl
                ;int g = ((n * n) % 256)
                ;mov edx, dword ptr [rsp + r8]; g
                ;imul edx,edx
                ;mov eax, edx
                ;idiv r9
                ;mov rax, rcx
                ;sub rax, r11; i-subTabBeginPoint
                ;imul rax, 3 ;3 * (i-subTabBeginPoint))
                ;inc rax ;3 * (i-subTabBeginPoint)) +1
                ;mov byte ptr[r10 + rax], dl
                ;int b = (n % 256)
                ;mov eax, edx
                ;idiv r9
                ;mov rax,rcx
                ;sub rax, r11; i-subTabBeginPoint
                ;imul rax, 3 ;3 * (i-subTabBeginPoint))
                ;inc rax ;3 * (i-subTabBeginPoint)) +1
                ;inc rax ;3 * (i-subTabBeginPoint)) +2
                ;mov byte ptr[r10 + rax], dl

                ;dec rbx

                ;obliczanie indeksu 3*(i-subTabBeginPoint)
                ;mov rax, rcx
                ;mov r15, subTabBeginPoint
                ;sub rax, r15
                ;imul rax, 3
                ;add rax, r9 ;dodanie do indeksacji tablicy jej polozenia w pamieci zdefiniowanego w rejestrze r9
                ;imageBuffer[3 * (i-subTabBeginPoint)] = b;
                ;mov dword ptr[rax + r10], r11d
			    ;imageBuffer[(3 * (i-subTabBeginPoint)) + 1] = g;
                ;mov dword ptr[rax + r10], r12d
			    ;imageBuffer[(3 * (i-subTabBeginPoint)) + 2] = r;
                ;mov dword ptr[rax + r10], r13d
                ;jnz WriteLoop
        inc r15
        add rsi, 4; i+= 4
        ;sprawdzenie warunku petli
        mov rbx, qword ptr [rbp+48]
        ; sizeofSubTable + subTabBeginPoint
        add rbx, r9
        cmp rsi,  rbx
        jl ComplexPlaneLoop
        
EndOfLoop:
       ;mov al, byte ptr [rbp-8] ;przeslanie adresu poczatkowego tablicy jako wynik dzialania funkcji (?) - moze niepotrzebne
       ;mov rbx, qword ptr [rbx-40] ;przywrocenie stanu rejestru rbx
       mov rax, r15
       ;pop rbx
       pop r15
       pop r14
       pop r13
       pop r12
       pop r11
       pop r10
       pop rbx
       pop rsi
       pop rdi
       ;mov rsp,rbp ;oproznienie stosu do stanu sprzed alokacji
       pop rbp; zdjecie ostatniego zaalokowanego recznie elementu
       ret 

generateMandelbrotFraktalAsm endp
end
;-------------------------------------------------------------------------
