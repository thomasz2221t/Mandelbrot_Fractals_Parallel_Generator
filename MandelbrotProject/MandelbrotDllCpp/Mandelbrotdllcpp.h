#pragma once

#ifdef MANDELBROTCPP_API_EXPORTS
#define MANDELBROTCPP_API __declspec(dllexport)
#else
#define MANDELBROTCPP_API __declspec(dllimport)
#endif

#include <windows.h>

extern "C" MANDELBROTCPP_API long  generateMandelbrotFraktalCpp(byte * imageBuffer, double* tableMappedToReal, double* tableMappedToImaginaris, long subTabBeginPoint, long sizeOfSubTable, long maxIteration);

/** Funkcja wyznacza ilosc iteracji na podstawie ktorej okresliamy czy dany punkt na plaszczyznie zespolonej nalezy do zbioru mandelbrota czy nie. Dany punkt przynalezy do zbioru mandelbrota jezeli wynik rownania rekurencyjnego. Wszystkie punkty nalezace do zbioru mandelbrota musza byc w odleglosci 2 od srodka plaszczyzny. W zaleznosci od liczby iteracji nakladane beda funkcje kolorujace definiujace przynaleznosc do zbioru mandelbrota lub niezawieranie siê w zbiorze mandelbrota - tutaj rowniez wplyw ma ilosc iteracji.
* {z0 = 0
* {zn+1 = zn^2 + p, gdzie p jest liczba zespolona p = {cRealis, cImaginaris}
* nie dazy do nieskonczonosci (spelnione sa warunki: nie przekoroczono maksymalnej liczby iteracji oraz modul liczby zespolonej zn nie przekracza 2)
* @param cRealis - wartosc rzeczywista liczby zespolonej rozwazanego pixela
* @param cImaginaris - wartosc urojona liczby zespolonej rozwazanego pixela
* @param max_iteracji - ustalona maksymalna liczba iteracji
*/
//int znajdzWartoscWZbiorzeMandelbrota(double cRealis, double cImaginaris, long max_iteracji);

/** Funkcja dokonuje konwersji wartosci pixela (a tak naprawde komorki naszej tablicy) do liczby rzeczywistej jaka pixel ma na plaszczyznie zespolonej. Na poczatku liczymy zakres liczb rzeczywistych na plaszczyznie (maxR - minR), a nastepnie dzielimy ten zakres przez rozdzielczosc pozioma okna - otrzymujemy wspolczynnik przeskalowania dla kazdego pixela. Mnozymy wspolczynnik przez wartosc x tabeli - otrzymujemy wartosc pixela przy poczatku liczb zespolonych od 0. Nastepnie dodajemy poprawke minR aby wartosc pixela obliczona dla plaszczyzny zespolonej z poczatkiem w 0, odpowiadala naszej plaszczyznie zespolonej.
* @param x - wartosc x komorki tablicy reprezentujaca pixele
* @param resolutionX - rozdzielczosc pozioma okna
* @param minR - minimalna wartosc rzeczywista plaszczyzyny zespolonej
* @param maxR - maksymalna wartosc rzeczywista plaszczyzny zespolonej
*/
//double mapToReal(int x, int resolutionX, double minR, double maxR);

/** Funckja dokonuje konwersji wartosci pixela do liczby urojonej jaka pixel ma na plaszczyznie zespolonej. Najpierw liczymy zakres jakie maja liczby urojone na plaszczyznie (maxI - minI), a nastepnie dzielimy ten zakres przez rozdzielczosc pionowa okna - otrzymujemy wspolczynnik przeskalowania dla kazdego pixela. Mnozymy wspolczynnik przez wartosc y tabeli (pionowa pozycje komorki) i otrzymujemy wartosc pixela przy plaszczyznie zespolonej zaczynajacej sie w pionie od 0. Nastepnie dodajemy poprawke minI aby wartosc pixela obliczona byla dla plaszczyzny zespolonej ktora wybralismy.
* @param y - wartosc y komorki tablicy reprezentujaca pixele
* @param resolutionY - rozdzielczosc pionowa okna
* @param minI - minimalna wartosc urojona plaszczyzny zespolonej
* @param maxI - maksymalna wartosc rzeczywista plaszczyzny zespolonej
*/
//double mapToImaginaris(int y, int resolutionY, double minI, double maxI);