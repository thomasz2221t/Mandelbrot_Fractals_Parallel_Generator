// JAApp.cpp : Defines the entry point for the console application.
//
#include <windows.h>
#include <iostream>

#include "JAApp.h"

//extern "C" int _stdcall MyProc1(DWORD x, DWORD y);

typedef int(_stdcall* LPFNDLLFUNC1)(DWORD, DWORD);
//typedef int(_stdcall* LPFNDLLFUNC2)(void);

int main()
{
	int x = 3;
	int y = 4;
	int z = 0;
	HINSTANCE hDLL = LoadLibrary(L"C:\\Users\\0_0\\Documents\\Assembler projekt\\tc2221t_assembler_mandelbrot\\JASol\\Debug\\JADll.dll");
	//HINSTANCE hDLL = LoadLibrary(L"$ProjectDir\\Debug\\JADll.dll");
	LPFNDLLFUNC1 lpfnDllFunc1;
	//LPFNDLLFUNC2 lpfnDllFunc2;
	if (NULL != hDLL)
	{
		lpfnDllFunc1 = (LPFNDLLFUNC1)GetProcAddress(hDLL, "MyProc1");
		if (NULL != lpfnDllFunc1)
		{
			z = lpfnDllFunc1(x, y);
			std::cout << z << std::endl;
		}

		//lpfnDllFunc2 = (LPFNDLLFUNC2)GetProcAddress(hDLL, "MyProc2");
		//if (NULL != lpfnDllFunc2)
		//{
		//	lpfnDllFunc2();
		//}
	}

	FreeLibrary(hDLL);

	return 0;
}
