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
        public bool czy_wykonano_bicie = false;
        private int ilosc_zbitych_biale, ilosc_zbitych_czarne;
        public bool Czy_Wygraly_Biale
        { 
            get
            {
                return ilosc_zbitych_czarne == 12;
            }
        }

        public bool Czy_Wygraly_Czarne
        {
            get
            {
                return ilosc_zbitych_biale == 12;
            }
        }
        public Szachownica()
        {
            Pozycja p = new Pozycja(0, 0);
            _szachownica = new Pionek[8,8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    _szachownica[i, j] = null;
            // pionki w pierwszych 3 wierszach i 3 ostatnich
            for (int i_wiersz = 0, x = 0; i_wiersz < 3; i_wiersz++)
            {
                for (int i_kolumna = 0; i_kolumna < 8; i_kolumna += 2)
                {
                    p.Wiersz = i_wiersz;
                    p.Kolumna = i_kolumna + x % 2;
                    _szachownica[i_wiersz, p.Kolumna] = new Pionek(true, p, false);
                }
                x++;
            }
            for (int i_wiersz = 5, x = 1; i_wiersz < 8; i_wiersz++)
            {
                for (int i_kolumna = 0; i_kolumna < 8; i_kolumna += 2)
                {
                    p.Wiersz = i_wiersz;
                    p.Kolumna = i_kolumna + x % 2;
                    _szachownica[i_wiersz, p.Kolumna] = new Pionek(false, p, false);
                }
                x++;
            }
            ilosc_zbitych_biale = 0;
            ilosc_zbitych_czarne = 0;
        }

        public Pionek[,] szachownica
        {
            get
            {
                return _szachownica;
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
        public bool Czy_Jest_Mozliwe_Kolejne_Bicie(Pozycja skad)
        {
            if(_szachownica[skad.Wiersz, skad.Kolumna].Czy_Dama)
                return Czy_Jest_Mozliwe_Kolejne_Bicie_Dama(skad);
            else
                return Czy_Jest_Mozliwe_Kolejne_Bicie_Pionek(skad);
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
                        {
                            if (_szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                                ilosc_zbitych_biale++;
                            else
                                ilosc_zbitych_czarne++;
                            Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                            czy_wykonano_bicie = true;
                            return Zmien_Pozycje_Pionka(skad, dokad);
                        }
                    }
                    return Zmien_Pozycje_Pionka(skad, dokad);
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
                        {
                            if (_szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                                ilosc_zbitych_biale++;
                            else
                                ilosc_zbitych_czarne++;
                            Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                            czy_wykonano_bicie = true;
                            return Zmien_Pozycje_Pionka(skad, dokad);
                        }
                    }
                    return Zmien_Pozycje_Pionka(skad, dokad);
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
                        {
                            if (_szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                                ilosc_zbitych_biale++;
                            else
                                ilosc_zbitych_czarne++;
                            Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                            czy_wykonano_bicie = true;
                            return Zmien_Pozycje_Pionka(skad, dokad);
                        }
                    }
                    return Zmien_Pozycje_Pionka(skad, dokad);
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
                            {
                                if (_szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                                    ilosc_zbitych_biale++;
                                else
                                    ilosc_zbitych_czarne++;
                                Usun_Pionek_Z_Planszy(new Pozycja(i_wiersz, i_kolumna));
                                czy_wykonano_bicie = true;
                                return Zmien_Pozycje_Pionka(skad, dokad);
                            }
                        }
                    return Zmien_Pozycje_Pionka(skad, dokad);
                }
        }

        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_DL(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            int ilosc_pionkow_przeciwnika = 0;
            for (int i_wiersz = skad.Wiersz - 1, i_kolumna = skad.Kolumna - 1; i_wiersz > dokad.Wiersz && i_kolumna > dokad.Kolumna; i_wiersz--, i_kolumna--)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                {
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
                    ilosc_pionkow_przeciwnika++;
                }
            if (ilosc_pionkow_przeciwnika > 1)
                return true;
            return false;
        }

        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_DP(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            int ilosc_pionkow_przeciwnika = 0;
            for (int i_wiersz = skad.Wiersz - 1, i_kolumna = skad.Kolumna + 1; i_wiersz > dokad.Wiersz && i_kolumna < dokad.Kolumna; i_wiersz--, i_kolumna++)
                if (_szachownica[i_wiersz, i_kolumna] != null)            
                {
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
                    ilosc_pionkow_przeciwnika++;
                }
            if (ilosc_pionkow_przeciwnika > 1)
                return true;
            return false;
        }

        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_GL(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            int ilosc_pionkow_przeciwnika = 0;
            for (int i_wiersz = skad.Wiersz + 1, i_kolumna = skad.Kolumna - 1; i_wiersz < dokad.Wiersz && i_kolumna > dokad.Kolumna; i_wiersz++, i_kolumna--)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                {
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
                    ilosc_pionkow_przeciwnika++;
                }
            if (ilosc_pionkow_przeciwnika > 1)
                return true;

            return false;
        }
        private bool Czy_Stoi_Pionek_Tego_Samego_Koloru_GP(Pozycja skad, Pozycja dokad)
        {
            Pionek p = _szachownica[skad.Wiersz, skad.Kolumna];
            int ilosc_pionkow_przeciwnika = 0;
            for (int i_wiersz = skad.Wiersz + 1, i_kolumna = skad.Kolumna + 1; i_wiersz < dokad.Wiersz && i_kolumna < dokad.Kolumna; i_wiersz++, i_kolumna++)
                if (_szachownica[i_wiersz, i_kolumna] != null)
                {
                    if (p.Czy_Bialy() == _szachownica[i_wiersz, i_kolumna].Czy_Bialy())
                        return true;
                    ilosc_pionkow_przeciwnika++;
                }
            if (ilosc_pionkow_przeciwnika > 1)
                return true;
            return false;
        }
        private bool Czy_Ruch_Pionka_Jest_Poprawny(Pozycja skad, Pozycja dokad)
        {
            if (_szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
            {
                if (Czy_Ruch_Bialego_Pionka_Jest_Poprawny(skad, dokad))
                {
                    if (dokad.Wiersz == 7)
                        _szachownica[dokad.Wiersz, dokad.Kolumna].Czy_Dama = true;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (Czy_Ruch_Czarnego_Pionka_Jest_Poprawny(skad, dokad))
                {
                    if (dokad.Wiersz == 0)
                        _szachownica[dokad.Wiersz, dokad.Kolumna].Czy_Dama = true;
                    return true;
                }
                else
                    return false;
            }
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
            lewo_dol = (skad.Wiersz - 1) == dokad.Wiersz && (skad.Kolumna - 1) == dokad.Kolumna;
            prawo_dol = (skad.Wiersz - 1) == dokad.Wiersz && (skad.Kolumna + 1) == dokad.Kolumna;
            lewo_gora = (skad.Wiersz + 1) == dokad.Wiersz && (skad.Kolumna - 1) == dokad.Kolumna;
            prawo_gora = (skad.Wiersz + 1) == dokad.Wiersz && (skad.Kolumna + 1) == dokad.Kolumna;

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

            // jesli ruch byl w lewo i w dol, a po srodku jest pionek innego koloru
            if (lewo_dol2)
                if (_szachownica[skad.Wiersz - 1, skad.Kolumna - 1] != null)
                    if (_szachownica[skad.Wiersz - 1, skad.Kolumna - 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        if (_szachownica[skad.Wiersz - 1, skad.Kolumna - 1].Czy_Bialy())
                            ilosc_zbitych_biale++;
                        else
                            ilosc_zbitych_czarne++;
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz - 1, skad.Kolumna - 1));
                        czy_wykonano_bicie = true;
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
                // jesli pole jest puste po srodku ruch jest niedozwolony
                else
                    return false;

            // jesli ruch byl w prawo i w dol, a po srodku jest pionek innego koloru
            if (prawo_dol2)
                if (_szachownica[skad.Wiersz - 1, skad.Kolumna + 1] != null)
                    if (_szachownica[skad.Wiersz - 1, skad.Kolumna + 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        if (_szachownica[skad.Wiersz - 1, skad.Kolumna + 1].Czy_Bialy())
                            ilosc_zbitych_biale++;
                        else
                            ilosc_zbitych_czarne++;
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz - 1, skad.Kolumna + 1));
                        czy_wykonano_bicie = true;
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
                // jesli pole jest puste po srodku ruch jest niedozwolony
                else
                    return false;

            // jesli ruch byl w lewo i w gore i nie stoi nic na tym polu, a po srodku jest pionek innego koloru
            if (lewo_gora2)
                if (_szachownica[skad.Wiersz + 1, skad.Kolumna - 1] != null)
                    if (_szachownica[skad.Wiersz + 1, skad.Kolumna - 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        if (_szachownica[skad.Wiersz + 1, skad.Kolumna - 1].Czy_Bialy())
                            ilosc_zbitych_biale++;
                        else
                            ilosc_zbitych_czarne++;
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz + 1, skad.Kolumna - 1));
                        czy_wykonano_bicie = true;
                        return Zmien_Pozycje_Pionka(skad, dokad);
                    }
                    // jesli pionek po srodku ma ten sam kolor ruch jest niedozwolony
                    else
                        return false;
                // jesli pole jest puste po srodku ruch jest niedozwolony
                else
                    return false;

            // jesli ruch byl w lewo i w gore i nie stoi nic na tym polu, a po srodku jest pionek innego koloru
            if (prawo_gora2)
                if (_szachownica[skad.Wiersz + 1, skad.Kolumna + 1] != null)
                    if (_szachownica[skad.Wiersz + 1, skad.Kolumna + 1].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    {
                        if (_szachownica[skad.Wiersz + 1, skad.Kolumna + 1].Czy_Bialy())
                            ilosc_zbitych_biale++;
                        else
                            ilosc_zbitych_czarne++;
                        Usun_Pionek_Z_Planszy(new Pozycja(skad.Wiersz + 1, skad.Kolumna + 1));
                        czy_wykonano_bicie = true;
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
            {
                _szachownica[p.Wiersz, p.Kolumna] = null;
            }
            return _szachownica[p.Wiersz, p.Kolumna] == null;
        }
        private bool Czy_Jest_Mozliwe_Kolejne_Bicie_Pionek(Pozycja skad)
        {
            bool lewo_dol = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Pionek(skad, -1, -1);
            bool prawo_dol = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Pionek(skad, 1, -1);
            bool lewo_gora = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Pionek(skad, -1, 1);
            bool prawo_gora = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Pionek(skad, 1, 1);
            return lewo_dol || prawo_dol || lewo_gora || prawo_gora;
        }

        private bool Czy_Jest_Mozliwe_Bicie_W_Kierunku_Pionek(Pozycja skad, int wiersz, int kolumna)
        {
            // jesli nie istnieje pole oddalone od pionka o 2 to nie mozna bic
            if(skad.Wiersz + 2 * wiersz < 0 || skad.Wiersz + 2 * wiersz > 7 || skad.Kolumna + 2 * kolumna < 0 || skad.Kolumna + 2 * kolumna > 7)
                return false;
            // jesli jest pion do bicia
            if (_szachownica[skad.Wiersz + wiersz, skad.Kolumna + kolumna] != null)
                // czy pion do bicia jest innego koloru		
                if (_szachownica[skad.Wiersz + wiersz, skad.Kolumna + kolumna].Czy_Bialy() != _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                    // czy pole za bitym pionkiem jest puste
                    if (_szachownica[skad.Wiersz + 2 * wiersz, skad.Kolumna + 2 * kolumna] == null)
                        return true;
            return false;
        }

        private bool Czy_Jest_Mozliwe_Kolejne_Bicie_Dama(Pozycja skad)
        {
            bool lewo_dol   = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Dama(skad, -1, -1);
            bool prawo_dol  = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Dama(skad, 1, -1);
            bool lewo_gora  = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Dama(skad, -1, 1);
            bool prawo_gora = Czy_Jest_Mozliwe_Bicie_W_Kierunku_Dama(skad, 1, 1);
            return lewo_dol || prawo_dol || lewo_gora || prawo_gora;
        }

        private bool Czy_Jest_Mozliwe_Bicie_W_Kierunku_Dama(Pozycja skad, int wiersz, int kolumna)
        {
            int Wiersz = skad.Wiersz, Kolumna = skad.Kolumna;
            for (; (Wiersz >= 0 && Wiersz <= 7) && (Kolumna >= 0 && Kolumna <= 7); Wiersz += wiersz, Kolumna += kolumna)
            {
                if (((Wiersz + 2 * wiersz) < 0) || ((Wiersz + 2 * wiersz) > 7) || ((Kolumna + 2 * kolumna) < 0) || ((Kolumna + 2 * kolumna) > 7))
                    return false;
                // jesli jest pion do bicia
                if (_szachownica[Wiersz + wiersz, Kolumna + kolumna] != null)
                    // czy pion do bicia jest tego samego koloru		
                    if (_szachownica[Wiersz + wiersz, Kolumna + kolumna].Czy_Bialy() == _szachownica[skad.Wiersz, skad.Kolumna].Czy_Bialy())
                        return false;
                    // czy pole za bitym pionkiem jest puste
                    else
                        if (_szachownica[Wiersz + 2 * wiersz, Kolumna + 2 * kolumna] == null)
                            return true;
                        // jesli nie jest puste to nie jest mozliwe bicie
                        else
                            return false;
            }
            return false;
        }
    }
}