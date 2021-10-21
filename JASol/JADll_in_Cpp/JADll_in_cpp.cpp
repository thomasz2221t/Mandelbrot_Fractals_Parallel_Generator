
#include <iostream>

#include "JADll_in_cpp.h"

#define JADLL_IN_CPP_API __declspec(dllexport)

extern "C" JADLL_IN_CPP_API int Procedura1(int x, int y)
{
	return x+y;
}