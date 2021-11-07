#pragma once

#ifdef MANDELBROTCPP_API_EXPORTS
#define MANDELBROTCPP_API __declspec(dllexport)
#else
#define MANDELBROTCPP_API __declspec(dllimport)
#endif

#include <windows.h>

extern "C" MANDELBROTCPP_API int generateMandelbrotFraktalCpp(byte* imageBuffer, int bufforLength, int bitmapPieceX, int bitmapPieceY);
