// JAApp.cpp : Defines the entry point for the console application.
//
#include <windows.h>
#include <iostream>

//#include "JAApp.h"

//extern "C" int _stdcall MyProc1(DWORD x, DWORD y);
extern "C" int Procedura1(int x, int y);

typedef int(_stdcall* LPFNDLLFUNC1)(DWORD, DWORD);
//typedef int(_stdcall* LPFNDLLFUNC2)(void);

typedef int(*LPFNDLLCPPFUNC1)(int, int);

int main()
{
	int x = 3;
	int y = 4;
	int z = 0;
	int z_c = 0;
	HINSTANCE hDLL = LoadLibrary(L"C:\\Users\\0_0\\Documents\\Assembler projekt\\tc2221t_assembler_mandelbrot\\JASol\\Debug\\JADll.dll");
	HINSTANCE hDLLCPP = LoadLibrary(L"C:\\Users\\0_0\\Documents\\Assembler projekt\\tc2221t_assembler_mandelbrot\\JASol\\Debug\\JADll_in_Cpp.dll");


	LPFNDLLFUNC1 lpfnDllFunc1;
	LPFNDLLCPPFUNC1 lpfnDllCppFunc1;
	//LPFNDLLFUNC2 lpfnDllFunc2;

	if ((NULL != hDLL)&&(NULL != hDLLCPP))
	{
		lpfnDllFunc1 = (LPFNDLLFUNC1)GetProcAddress(hDLL, "MyProc1");
		if (NULL != lpfnDllFunc1)
		{
			z = lpfnDllFunc1(x, y);
			std::cout << z << std::endl;
		}
		else
		{
			std::cout << "Nie znaleziono funkcji MyProc1 w bibliotece\n";
		}

		lpfnDllCppFunc1 = (LPFNDLLCPPFUNC1)GetProcAddress(hDLLCPP, "Procedura1");
		if (NULL != lpfnDllCppFunc1)
		{
			std::cout << "Dziala\n" << std::endl;
			z_c = lpfnDllCppFunc1(x, y);
			std::cout << z_c << std::endl;
		}
		else
		{
			std::cout << "Nie znaleziono funkcji Procedura1 w bibliotece\n";
		}

		//lpfnDllFunc2 = (LPFNDLLFUNC2)GetProcAddress(hDLL, "MyProc2");
		//if (NULL != lpfnDllFunc2)
		//{
		//	lpfnDllFunc2();
		//}
	}

	FreeLibrary(hDLL);
	FreeLibrary(hDLLCPP);

	return 0;
}
