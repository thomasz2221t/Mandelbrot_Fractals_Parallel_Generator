#ifndef MANDELBROTCPP_H
#define MANDELBROTCPP_H

#ifdef MANDELBROTCPP_API_EXPORTS
#define MANDELBROTCPP_API __declspec(dllexport)
#else
#define MANDELBROTCPP_API __declspec(dllimport)
#endif

#include <windows.h>

extern "C" MANDELBROTCPP_API int Procedura1(int x, int y);

#endif
