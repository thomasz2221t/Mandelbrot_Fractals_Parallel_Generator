// JAApp.cpp : Defines the entry point for the console application.
//
#include <windows.h>
#include <iostream>

#include "JAApp.h"

//extern "C" int _stdcall MyProc1(DWORD x, DWORD y);


int main()
{
	int x = 3, y = 4, z = 0;

	z = MyProc1(x, y);  // call to library assemmbler procedure
	std::cout << z << std::endl;


	return 0;
}
