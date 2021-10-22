#include <iostream>

#include "MandelbrotCpp.h"

#define MANDELBROTCPP_API __declspec(dllexport)

extern "C" MANDELBROTCPP_API int Procedura1(int x, int y)
{
	return x + y;
}