#include <iostream>

#include"pch.h" 
#include "Mandelbrotdllcpp.h"
//#include "..\MandelbrotApp\*plik naglowkowy C#*"

#define MANDELBROTCPP_API __declspec(dllexport)


//while (x * x + y * y ≤ 2 * 2 AND iteration < max_iteration)
// {
//	xtemp : = x * x - y * y + x0
//	y : = 2 * x * y + y0
//	x : = xtemp
//	iteration : = iteration + 1
// }


extern "C" {

	MANDELBROTCPP_API int generateMandelbrotFraktalCpp(byte* imageBuffer, int bufforLength, int bitmapPieceX, int bitmapPieceY)
	{
		for (int i = 0; i < bufforLength; i++)
		{
			byte x = imageBuffer[i];
		}
			byte x = imageBuffer[0];
		return bufforLength;
	}

}
