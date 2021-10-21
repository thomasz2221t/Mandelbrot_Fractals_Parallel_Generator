#ifndef JADLL_IN_CPP_H
#define JADLL_IN_CPP_H

#ifdef JADLL_IN_CPP_EXPORTS
#define JADLL_IN_CPP_API __declspec(dllexport)
#else
#define JADLL_IN_CPP_API __declspec(dllimport)
#endif

#include <windows.h>

extern "C" JADLL_IN_CPP_API int Procedura1(int x, int y);









#endif
