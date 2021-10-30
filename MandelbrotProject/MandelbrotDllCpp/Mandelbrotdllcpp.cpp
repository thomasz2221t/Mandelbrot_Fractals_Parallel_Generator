#include <iostream>

#include"pch.h" 
#include "Mandelbrotdllcpp.h"
//#include "..\MandelbrotApp\*plik naglowkowy C#*"

#define MANDELBROTCPP_API __declspec(dllexport)

extern "C" {

	MANDELBROTCPP_API int Procedura1(int x, int y)
	{
		return x + y;
	}

}
