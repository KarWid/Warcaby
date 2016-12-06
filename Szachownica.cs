using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Warcaby;

namespace Warcaby
{
    public class Szachownica
    {
        private Pionek[,] _szachownica;
        private bool lewo_dol, prawo_dol, lewo_gora, prawo_gora, lewo_dol2, prawo_dol2, lewo_gora2, prawo_gora2;
        public Szachownica()
        {
            _szachownica = new Pionek[8,8];
            Pozycja p = new Pozycja(0, 0);
            for (int i_wiersz = 0, x = 0; i_wiersz < 3; i_wiersz++)
            {
                for (int i_kolumna = 0; i_kolumna < 8; i_kolumna += 2)
                {
                    p.Wiersz = i_wiersz;
                    p.Kolumna = i_kolumna + x % 2;
                    _szachownica[i_wiersz, i_kolumna] = new Pionek(true, p, false);
                }
                x++;
            }
            for (int i_wiersz = 5, x = 1; i_wiersz < 8; i_wiersz++)
            {
                for (int i_kolumna = 0; i_kolumna < 8; i_kolumna += 2)
                {
                    p.Wiersz = i_wiersz;
                    p.Kolumna = i_kolumna + x % 2;
                    _szachownica[i_wiersz, i_kolumna] = new Pionek(false, p, false);
                }
                x++;
            }
        }

        public bool Ruch(Pozycja skad, Pozycja dokad)
        // metoda sprawdzajaca czy ruch byl wykonany zgodnie z zasadami warcab i rozpoznaje czy jest to ruch wykonywany przez dame czy nie
        {
            // jesli ruch odbyl sie w pionie lub poziomie jest nieprawidlowy, lub wogole sie nie odbyl
            if (skad.Wiersz == dokad.Wiersz || skad.Kolumna == dokad.Kolumna)
                return false;

            // jesli stoi na miejscu docelowym jakis obiekt ruch takze jest nieprawidlowy
            if (_szachownica[dokad.Wiersz, dokad.Kolumna] != null)
                return false;

            if (_szachownica[skad.Wiersz, skad.Kolumna].Czy_Dama)
                return Czy_Ruch_Damy_Jest_Poprawny(skad, dokad);
            else
                return Czy_Ruch_Pionka_Jest_Poprawny(skad, dokad);
        }

        private bool Czy_Ruch_Damy_Jest_Poprawny(Pozycja skad, Pozycja dokad)
        {
            int i_wiersz, i_kolumna;
            // ruch w dol
            if(skad.Wiersz > dokad.Wiersz)
                // ruch w lewo
                if(skad.Kolumna > dokad.Kolumna)
                {
                    if(Czy_Stoi_Pionek_Tego_Samego_Koloru_DL(skad, dokad))
                        return false;
                    else
                    for(i_wiersz = skad.Wiersz - 1, i_kolumna = skad.Kolumna - 1; i_wiersz > dokad.Wiersz && i_kolumna > dokad.Kolumna; i_wiersz--, i_kolumna--)
                    {
                        if (_szachownica[i_wiersz, i_kolumna] != null)
                            Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                    }
                    return true;
                }
                // ruch w prawo
                else
                {
                    if(Czy_Stoi_Pionek_Tego_Samego_Koloru_DP(skad, dokad))
                        return false;
                    else
                    for (i_wiersz = skad.Wiersz - 1, i_kolumna = skad.Kolumna + 1; i_wiersz > dokad.Wiersz && i_kolumna < dokad.Kolumna; i_wiersz--, i_kolumna++)
                    {
                        if (_szachownica[i_wiersz, i_kolumna] != null)
                            Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                    }
                    return true;
                }
            // ruch w gore
            else
                // ruch w lewo
                if (skad.Kolumna > dokad.Kolumna)
                {
                    if(Czy_Stoi_Pionek_Tego_Samego_Koloru_GL(skad, dokad))
                        return false;
                    else
                    for (i_wiersz = skad.Wiersz + 1, i_kolumna = skad.Kolumna - 1; i_wiersz < dokad.Wiersz && i_kolumna > dokad.Kolumna; i_wiersz++, i_kolumna--)
                    {
                        if (_szachownica[i_wiersz, i_kolumna] != null)
                            Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                    }
                    return true;
                }
                // ruch w prawo
                else
                {
                    if(Czy_Stoi_Pionek_Tego_Samego_Koloru_GP(skad, dokad))
                        return false;
                    else
                        for (i_wiersz = skad.Wiersz + 1, i_kolumna = skad.Kolumna + 1; i_wiersz < dokad.Wiersz && i_kolumna < dokad.Kolumna; i_wiersz++, i_kolumna++)
                        {
                            if (_szachownica[i_wiersz, i_kolumna] != null)
                                Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                        }
                    return true;
                }
        }

        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_DL(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            for (int i_wiersz = skad.Wiersz - 1, i_kolumna = skad.Kolumna - 1; i_wiersz > dokad.Wiersz && i_kolumna > dokad.Kolumna; i_wiersz--, i_kolumna--)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
            return false;
        }

        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_DP(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            for (int i_wiersz = skad.Wiersz - 1, i_kolumna = skad.Kolumna + 1; i_wiersz > dokad.Wiersz && i_kolumna < dokad.Kolumna; i_wiersz--, i_kolumna++)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
            return false;
        }

        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_GL(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            for (int i_wiersz = skad.Wiersz + 1, i_kolumna = skad.Kolumna - 1; i_wiersz < dokad.Wiersz && i_kolumna > dokad.Kolumna; i_wiersz++, i_kolumna--)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
            return false;
        }
        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_GP(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            for (int i_wiersz = skad.Wiersz + 1, i_kolumna = skad.Kolumna + 1; i_wiersz < dokad.Wiersz && i_kolumna < dokad.Kolumna; i_wiersz++, i_kolumna++)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
            return false;
        }
        private bool Czy_Ruch_Pionka_Jest_Poprawny(Pozycja skad, Pozycja dokad)
        {
            if (_szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                return Czy_Ruch_Bialego_Pionka_Jest_Poprawny(skad, dokad);
            else
                return Czy_Ruch_Czarnego_Pionka_Jest_Poprawny(skad, dokad);
        }

        private bool Czy_Ruch_Bialego_Pionka_Jest_Poprawny(Pozycja skad, Pozycja dokad)
        {
            // Sprawdzenie czy pionek przesuwa sie o 1 pozycje w odpowiednia strone
            lewo_dol = skad.Wiersz - 1 == dokad.Wiersz && skad.Kolumna - 1 == dokad.Kolumna;
            prawo_dol = skad.Wiersz - 1 == dokad.Wiersz && skad.Kolumna + 1 == dokad.Kolumna;
            lewo_gora = skad.Wiersz + 1 == dokad.Wiersz && skad.Kolumna - 1 == dokad.Kolumna;
            prawo_gora = skad.Wiersz + 1 == dokad.Wiersz && skad.Kolumna + 1 == dokad.Kolumna;

            // jesli to bialy to nie moze sie poruszyc w dol
            if (lewo_dol || prawo_dol)
                return false;

            // na miejscu docelowym nie ma pionka, ruch jest dozwolony w gore o jedna pozycje w lewo i prawo
            if (lewo_gora || prawo_gora)
                return Zmien_Pozycje_Pionka(skad, dokad);

            return Czy_Ruch_Podwojny(skad, dokad);
        }
        private bool Czy_Ruch_Czarnego_Pionka_Jest_Poprawny(Pozycja skad, Pozycja dokad)
        {
            lewo_dol = skad.Wiersz - 1 == dokad.Wiersz && skad.Kolumna - 1 == dokad.Kolumna;
            prawo_dol = skad.Wiersz - 1 == dokad.Wiersz && skad.Kolumna + 1 == dokad.Kolumna;
            lewo_gora = skad.Wiersz + 1 == dokad.Wiersz && skad.Kolumna - 1 == dokad.Kolumna;
            prawo_gora = skad.Wiersz + 1 == dokad.Wiersz && skad.Kolumna + 1 == dokad.Kolumna;

            // jesli to czarny to nie moze sie poruszyc w gore
            if (lewo_gora || prawo_gora)
                return false;

            // na miejscu docelowym nie ma pionka, ruch jest dozwolony w dol o jedna pozycje w lewo i prawo
            if (lewo_dol || prawo_dol)
                return Zmien_Pozycje_Pionka(skad, dokad);

            return Czy_Ruch_Podwojny(skad, dokad);
        }

        private bool Czy_Ruch_Podwojny(Pozycja skad, Pozycja dokad)
        {
            // Jesli ruch wystapil o 2 pola
            lewo_dol2 = skad.Wiersz - 2 == dokad.Wiersz && skad.Kolumna - 2 == dokad.Kolumna;
            prawo_dol2 = skad.Wiersz - 2 == dokad.Wiersz && skad.Kolumna + 2 == dokad.Kolumna;
            lewo_gora2 = skad.Wiersz + 2 == dokad.Wiersz && skad.Kolumna - 2 == dokad.Kolumna;
            prawo_gora2 = skad.Wiersz + 2 == dokad.Wiersz && skad.Kolumna + 2 == dokad.Kolumna;

            // jesli ruch byl w lewo i w dol i nie stoi nic na tym polu, a po srodku jest pionek innego koloru
            if (lewo_dol2 && _szachownica[skad.Wiersz - 2, skad.Kolumna - 2] == null)
                if (_szachownica[skad.Wiersz - 1, skad.Kolumna - 1] != null)
                    if (_szachownica[skad.Wiersz - 1, skad.Kolumna - 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz - 1, skad.Kolumna - 1));
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
                // jesli pole jest puste po srodku ruch jest niedozwolony
                else
                    return false;

            // jesli ruch byl w prawo i w dol i nie stoi nic na tym polu, a po srodku jest pionek innego koloru
            if (prawo_dol2 && _szachownica[skad.Wiersz - 2, skad.Kolumna + 2] == null)
                if (_szachownica[skad.Wiersz - 1, skad.Kolumna + 1] != null)
                    if (_szachownica[skad.Wiersz - 1, skad.Kolumna + 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz - 1, skad.Kolumna + 1));
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
                // jesli pole jest puste po srodku ruch jest niedozwolony
                else
                    return false;

            // jesli ruch byl w lewo i w gore i nie stoi nic na tym polu, a po srodku jest pionek innego koloru
            if (lewo_gora2 && _szachownica[skad.Wiersz + 2, skad.Kolumna - 2] == null)
                if (_szachownica[skad.Wiersz + 1, skad.Kolumna - 1] != null)
                    if (_szachownica[skad.Wiersz + 1, skad.Kolumna - 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz + 1, skad.Kolumna - 1));
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
                // jesli pole jest puste po srodku ruch jest niedozwolony
                else
                    return false;

            // jesli ruch byl w lewo i w gore i nie stoi nic na tym polu, a po srodku jest pionek innego koloru
            if (prawo_gora2 && _szachownica[skad.Wiersz + 2, skad.Kolumna + 2] == null)
                if (_szachownica[skad.Wiersz + 1, skad.Kolumna + 1] != null)
                    if (_szachownica[skad.Wiersz + 1, skad.Kolumna + 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz + 1, skad.Kolumna + 1));
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
            return false;
        }
        private bool Zmien_Pozycje_Pionka(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            if (Usun_Pionek_Z_Planszy(skad))
            {
                _szachownica[dokad.Wiersz, dokad.Kolumna] = p;
                return true;
            }
            else
                return false;
        }

        private bool Usun_Pionek_Z_Planszy(Pozycja p)
        {
            if(_szachownica[p.Wiersz, p.Kolumna] != null)
                _szachownica[p.Wiersz, p.Kolumna] = null;
            return _szachownica[p.Wiersz, p.Kolumna] == null;
        }

        public bool Czy_Mozliwy_Jest_Jeszcze_Ruch_Do_Wykonania(Pozycja skad, bool czy_bialy)
        {
            return true;
        }
    }
}