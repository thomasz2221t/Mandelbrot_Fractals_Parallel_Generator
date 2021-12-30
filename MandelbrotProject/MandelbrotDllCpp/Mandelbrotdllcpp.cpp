#include <iostream>
#include <math.h>

#include"pch.h" 
#include "Mandelbrotdllcpp.h"

#define MANDELBROTCPP_API __declspec(dllexport)


//while (x * x + y * y ≤ 2 * 2 AND iteration < max_iteration)
// {
//	xtemp : = x * x - y * y + x0
//	y : = 2 * x * y + y0
//	x : = xtemp
//	iteration : = iteration + 1
// }



extern "C" {

	/** Główna funkcja wykonujaca algorytm okreslajacy przynaleznosc do zbioru mandelbrota danego pixela oraz nakladajaca funkcje kolorujace.
	* @param imageBuffer - tablica reprezentujaca abstract pixeli bitmapy
	* @param bitMapX - wartosc poczatkowa tablicy, ktora ma zostac obsluzona w tym watku
	* @param bitMapY - wartosc koncowa tablicy, ktora ma zostac obsluzona w tym watku
	* @param dimensionX - wartosc pozioma rozdzielczosci okna 
	* @param dimensionY - wartosc pionowa rozdzielczosci okna
	* @param maxIteration - maksymalna liczba iteracji
	* @param numThreads - ilosc watkow zadeklarowana przez uzytkownika
	* @param minR - minimalna wartosc rzeczywista plaszczyzny zespolonej
	* @param maxR - maksymalna wartosc rzeczywista plaszczyzny zespolonej
	* @param minI - minimalna wartosc urojona plaszczyzny zespolonej
	* @param maxI - maksymalna wartosc urojona plaszczyzny zespolonej
	*/
	MANDELBROTCPP_API long  generateMandelbrotFraktalCpp(byte* imageBuffer, long subTabBeginPoint, long sizeOfSubTable, int dimensionX, int dimensionY, long maxIteration, double minR, double maxR, double minI, double maxI)
	{
		long operations = 0;
		for (long i = subTabBeginPoint; i < (sizeOfSubTable+subTabBeginPoint); i++)
		{
			int x = i % dimensionX;
			int y = i / dimensionX;
			//znalezienie wartosci realisa i imaginarisa dla kazdego piksla
			double cReal = mapToReal(x, dimensionX, minR, maxR);
			double cImaginaris = mapToImaginaris(y, dimensionY, minI, maxI);

			//Znalezienie wartosci zbioru mandelbrota dla danego piksla
			int n = znajdzWartoscWZbiorzeMandelbrota(cReal, cImaginaris, maxIteration);

			//nadanie wartosc rgb
			//int r = ((int)(i * sinf(i)) % 256); <- C3861 nie wiem czemu ale nie umie znalezc sinf
			int r = ((int)(n * 2 * n) % 256);
			int g = ((n * n) % 256);
			int b = (n % 256);

			imageBuffer[3 * (i-subTabBeginPoint)] = b;
			imageBuffer[(3 * (i-subTabBeginPoint)) + 1] = g;
			imageBuffer[(3 * (i-subTabBeginPoint)) + 2] = r;
			operations++;
		}
		return operations;
	}
}

/** Funkcja wyznacza ilosc iteracji na podstawie ktorej okresliamy czy dany punkt na plaszczyznie zespolonej nalezy do zbioru mandelbrota czy nie. Dany punkt przynalezy do zbioru mandelbrota jezeli wynik rownania rekurencyjnego. Wszystkie punkty nalezace do zbioru mandelbrota musza byc w odleglosci 2 od srodka plaszczyzny. W zaleznosci od liczby iteracji nakladane beda funkcje kolorujace definiujace przynaleznosc do zbioru mandelbrota lub niezawieranie się w zbiorze mandelbrota - tutaj rowniez wplyw ma ilosc iteracji.
* {z0 = 0
* {zn+1 = zn^2 + p, gdzie p jest liczba zespolona p = {cRealis, cImaginaris}
* nie dazy do nieskonczonosci (spelnione sa warunki: nie przekoroczono maksymalnej liczby iteracji oraz modul liczby zespolonej zn nie przekracza 2)
* @param cRealis - wartosc rzeczywista liczby zespolonej rozwazanego pixela
* @param cImaginaris - wartosc urojona liczby zespolonej rozwazanego pixela
* @param max_iteracji - ustalona maksymalna liczba iteracji
*/
int znajdzWartoscWZbiorzeMandelbrota(double cRealis, double cImaginaris, long max_iteracji)
{
	int i = 0;
	double zRealis = 0.0, zImaginaris = 0.0;
	while ((i < max_iteracji) && (zRealis * zRealis + zImaginaris * zImaginaris <= 4.0))
	{
		double bufor = zRealis * zRealis - zImaginaris * zImaginaris + cRealis;
		zImaginaris = 2.0 * zRealis * zImaginaris + cImaginaris;
		zRealis = bufor;
		i++;
	}

	return i;//zwracamy wartosc iteracji ktore przeszly
}


/** Funkcja dokonuje konwersji wartosci pixela (a tak naprawde komorki naszej tablicy) do liczby rzeczywistej jaka pixel ma na plaszczyznie zespolonej. Na poczatku liczymy zakres liczb rzeczywistych na plaszczyznie (maxR - minR), a nastepnie dzielimy ten zakres przez rozdzielczosc pozioma okna - otrzymujemy wspolczynnik przeskalowania dla kazdego pixela. Mnozymy wspolczynnik przez wartosc x tabeli - otrzymujemy wartosc pixela przy poczatku liczb zespolonych od 0. Nastepnie dodajemy poprawke minR aby wartosc pixela obliczona dla plaszczyzny zespolonej z poczatkiem w 0, odpowiadala naszej plaszczyznie zespolonej.
* @param x - wartosc x komorki tablicy reprezentujaca pixele
* @param resolutionX - rozdzielczosc pozioma okna
* @param minR - minimalna wartosc rzeczywista plaszczyzyny zespolonej
* @param maxR - maksymalna wartosc rzeczywista plaszczyzny zespolonej
*/
double mapToReal(int x, int resolutionX, double minR, double maxR)
{
	double range = maxR - minR;

	return x * (range / resolutionX) + minR;
}

/** Funckja dokonuje konwersji wartosci pixela do liczby urojonej jaka pixel ma na plaszczyznie zespolonej. Najpierw liczymy zakres jakie maja liczby urojone na plaszczyznie (maxI - minI), a nastepnie dzielimy ten zakres przez rozdzielczosc pionowa okna - otrzymujemy wspolczynnik przeskalowania dla kazdego pixela. Mnozymy wspolczynnik przez wartosc y tabeli (pionowa pozycje komorki) i otrzymujemy wartosc pixela przy plaszczyznie zespolonej zaczynajacej sie w pionie od 0. Nastepnie dodajemy poprawke minI aby wartosc pixela obliczona byla dla plaszczyzny zespolonej ktora wybralismy. 
* @param y - wartosc y komorki tablicy reprezentujaca pixele
* @param resolutionY - rozdzielczosc pionowa okna
* @param minI - minimalna wartosc urojona plaszczyzny zespolonej
* @param maxI - maksymalna wartosc rzeczywista plaszczyzny zespolonej
*/
double mapToImaginaris(int y, int resolutionY, double minI, double maxI)
{
	double range = maxI - minI;
	return y * (range / resolutionY) + minI;
}
