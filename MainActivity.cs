using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;

namespace Warcaby
{
    [Activity(Label = "Warcaby", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool czy_bialy_gracz;// kolor aktywnego gracza
        private bool czy_pierwsze_klikniecie;
        private Pozycja pozycja, pozycja_dokad;
        private Szachownica szachownica; // szachownica
        private Button[,] plansza; // plansza w widoku jako wirtualna tablica
        private Toast toast;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            // Get our ImageImageImageButton from the layout resource,
            // and attach an event to it
            plansza = new Button[8,8];
            szachownica = new Szachownica();
            pozycja_dokad = new Pozycja(0, 0);
            pozycja = new Pozycja(0, 0);

            plansza[0, 0] = FindViewById<Button>(Resource.Id.field_00);
            plansza[0, 1] = FindViewById<Button>(Resource.Id.field_01);
            plansza[0, 2] = FindViewById<Button>(Resource.Id.field_02);
            plansza[0, 3] = FindViewById<Button>(Resource.Id.field_03);
            plansza[0, 4] = FindViewById<Button>(Resource.Id.field_04);
            plansza[0, 5] = FindViewById<Button>(Resource.Id.field_05);
            plansza[0, 6] = FindViewById<Button>(Resource.Id.field_06);
            plansza[0, 7] = FindViewById<Button>(Resource.Id.field_07);
            plansza[1, 0] = FindViewById<Button>(Resource.Id.field_10);
            plansza[1, 1] = FindViewById<Button>(Resource.Id.field_11);
            plansza[1, 2] = FindViewById<Button>(Resource.Id.field_12);
            plansza[1, 3] = FindViewById<Button>(Resource.Id.field_13);
            plansza[1, 4] = FindViewById<Button>(Resource.Id.field_14);
            plansza[1, 5] = FindViewById<Button>(Resource.Id.field_15);
            plansza[1, 6] = FindViewById<Button>(Resource.Id.field_16);
            plansza[1, 7] = FindViewById<Button>(Resource.Id.field_17);
            plansza[2, 0] = FindViewById<Button>(Resource.Id.field_20);
            plansza[2, 1] = FindViewById<Button>(Resource.Id.field_21);
            plansza[2, 2] = FindViewById<Button>(Resource.Id.field_22);
            plansza[2, 3] = FindViewById<Button>(Resource.Id.field_23);
            plansza[2, 4] = FindViewById<Button>(Resource.Id.field_24);
            plansza[2, 5] = FindViewById<Button>(Resource.Id.field_25);
            plansza[2, 6] = FindViewById<Button>(Resource.Id.field_26);
            plansza[2, 7] = FindViewById<Button>(Resource.Id.field_27);
            plansza[3, 0] = FindViewById<Button>(Resource.Id.field_30);
            plansza[3, 1] = FindViewById<Button>(Resource.Id.field_31);
            plansza[3, 2] = FindViewById<Button>(Resource.Id.field_32);
            plansza[3, 3] = FindViewById<Button>(Resource.Id.field_33);
            plansza[3, 4] = FindViewById<Button>(Resource.Id.field_34);
            plansza[3, 5] = FindViewById<Button>(Resource.Id.field_35);
            plansza[3, 6] = FindViewById<Button>(Resource.Id.field_36);
            plansza[3, 7] = FindViewById<Button>(Resource.Id.field_37);
            plansza[4, 0] = FindViewById<Button>(Resource.Id.field_40);
            plansza[4, 1] = FindViewById<Button>(Resource.Id.field_41);
            plansza[4, 2] = FindViewById<Button>(Resource.Id.field_42);
            plansza[4, 3] = FindViewById<Button>(Resource.Id.field_43);
            plansza[4, 4] = FindViewById<Button>(Resource.Id.field_44);
            plansza[4, 5] = FindViewById<Button>(Resource.Id.field_45);
            plansza[4, 6] = FindViewById<Button>(Resource.Id.field_46);
            plansza[4, 7] = FindViewById<Button>(Resource.Id.field_47);
            plansza[5, 0] = FindViewById<Button>(Resource.Id.field_50);
            plansza[5, 1] = FindViewById<Button>(Resource.Id.field_51);
            plansza[5, 2] = FindViewById<Button>(Resource.Id.field_52);
            plansza[5, 3] = FindViewById<Button>(Resource.Id.field_53);
            plansza[5, 4] = FindViewById<Button>(Resource.Id.field_54);
            plansza[5, 5] = FindViewById<Button>(Resource.Id.field_55);
            plansza[5, 6] = FindViewById<Button>(Resource.Id.field_56);
            plansza[5, 7] = FindViewById<Button>(Resource.Id.field_57);
            plansza[6, 0] = FindViewById<Button>(Resource.Id.field_60);
            plansza[6, 1] = FindViewById<Button>(Resource.Id.field_61);
            plansza[6, 2] = FindViewById<Button>(Resource.Id.field_62);
            plansza[6, 3] = FindViewById<Button>(Resource.Id.field_63);
            plansza[6, 4] = FindViewById<Button>(Resource.Id.field_64);
            plansza[6, 5] = FindViewById<Button>(Resource.Id.field_65);
            plansza[6, 6] = FindViewById<Button>(Resource.Id.field_66);
            plansza[6, 7] = FindViewById<Button>(Resource.Id.field_67);
            plansza[7, 0] = FindViewById<Button>(Resource.Id.field_70);
            plansza[7, 1] = FindViewById<Button>(Resource.Id.field_71);
            plansza[7, 2] = FindViewById<Button>(Resource.Id.field_72);
            plansza[7, 3] = FindViewById<Button>(Resource.Id.field_73);
            plansza[7, 4] = FindViewById<Button>(Resource.Id.field_74);
            plansza[7, 5] = FindViewById<Button>(Resource.Id.field_75);
            plansza[7, 6] = FindViewById<Button>(Resource.Id.field_76);
            plansza[7, 7] = FindViewById<Button>(Resource.Id.field_77);

            plansza[0, 0].Click += delegate { OnClicked(0, 0); };
            plansza[0, 1].Click += delegate { OnClicked(0, 1); };
            plansza[0, 2].Click += delegate { OnClicked(0, 2); };
            plansza[0, 3].Click += delegate { OnClicked(0, 3); };
            plansza[0, 4].Click += delegate { OnClicked(0, 4); };
            plansza[0, 5].Click += delegate { OnClicked(0, 5); };
            plansza[0, 6].Click += delegate { OnClicked(0, 6); };
            plansza[0, 7].Click += delegate { OnClicked(0, 7); };
            plansza[1, 0].Click += delegate { OnClicked(1, 0); };
            plansza[1, 1].Click += delegate { OnClicked(1, 1); };
            plansza[1, 2].Click += delegate { OnClicked(1, 2); };
            plansza[1, 3].Click += delegate { OnClicked(1, 3); };
            plansza[1, 4].Click += delegate { OnClicked(1, 4); };
            plansza[1, 5].Click += delegate { OnClicked(1, 5); };
            plansza[1, 6].Click += delegate { OnClicked(1, 6); };
            plansza[1, 7].Click += delegate { OnClicked(1, 7); };
            plansza[2, 0].Click += delegate { OnClicked(2, 0); };
            plansza[2, 1].Click += delegate { OnClicked(2, 1); };
            plansza[2, 2].Click += delegate { OnClicked(2, 2); };
            plansza[2, 3].Click += delegate { OnClicked(2, 3); };
            plansza[2, 4].Click += delegate { OnClicked(2, 4); };
            plansza[2, 5].Click += delegate { OnClicked(2, 5); };
            plansza[2, 6].Click += delegate { OnClicked(2, 6); };
            plansza[2, 7].Click += delegate { OnClicked(2, 7); };
            plansza[3, 0].Click += delegate { OnClicked(3, 0); };
            plansza[3, 1].Click += delegate { OnClicked(3, 1); };
            plansza[3, 2].Click += delegate { OnClicked(3, 2); };
            plansza[3, 3].Click += delegate { OnClicked(3, 3); };
            plansza[3, 4].Click += delegate { OnClicked(3, 4); };
            plansza[3, 5].Click += delegate { OnClicked(3, 5); };
            plansza[3, 6].Click += delegate { OnClicked(3, 6); };
            plansza[3, 7].Click += delegate { OnClicked(3, 7); };
            plansza[4, 0].Click += delegate { OnClicked(4, 0); };
            plansza[4, 1].Click += delegate { OnClicked(4, 1); };
            plansza[4, 2].Click += delegate { OnClicked(4, 2); };
            plansza[4, 3].Click += delegate { OnClicked(4, 3); };
            plansza[4, 4].Click += delegate { OnClicked(4, 4); };
            plansza[4, 5].Click += delegate { OnClicked(4, 5); };
            plansza[4, 6].Click += delegate { OnClicked(4, 6); };
            plansza[4, 7].Click += delegate { OnClicked(4, 7); };
            plansza[5, 0].Click += delegate { OnClicked(5, 0); };
            plansza[5, 1].Click += delegate { OnClicked(5, 1); };
            plansza[5, 2].Click += delegate { OnClicked(5, 2); };
            plansza[5, 3].Click += delegate { OnClicked(5, 3); };
            plansza[5, 4].Click += delegate { OnClicked(5, 4); };
            plansza[5, 5].Click += delegate { OnClicked(5, 5); };
            plansza[5, 6].Click += delegate { OnClicked(5, 6); };
            plansza[5, 7].Click += delegate { OnClicked(5, 7); };
            plansza[6, 0].Click += delegate { OnClicked(6, 0); };
            plansza[6, 1].Click += delegate { OnClicked(6, 1); };
            plansza[6, 2].Click += delegate { OnClicked(6, 2); };
            plansza[6, 3].Click += delegate { OnClicked(6, 3); };
            plansza[6, 4].Click += delegate { OnClicked(6, 4); };
            plansza[6, 5].Click += delegate { OnClicked(6, 5); };
            plansza[6, 6].Click += delegate { OnClicked(6, 6); };
            plansza[6, 7].Click += delegate { OnClicked(6, 7); };
            plansza[7, 0].Click += delegate { OnClicked(7, 0); };
            plansza[7, 1].Click += delegate { OnClicked(7, 1); };
            plansza[7, 2].Click += delegate { OnClicked(7, 2); };
            plansza[7, 3].Click += delegate { OnClicked(7, 3); };
            plansza[7, 4].Click += delegate { OnClicked(7, 4); };
            plansza[7, 5].Click += delegate { OnClicked(7, 5); };
            plansza[7, 6].Click += delegate { OnClicked(7, 6); };
            plansza[7, 7].Click += delegate { OnClicked(7, 7); };

            // ustawienie tła dla białych pól na stałe, ponieważ jest to niezmienne przez cala gre
            for (int i = 0; i < 8; i++)
                for (int j = (i + 1) % 2; j < 8; j += 2)
                    plansza[i, j].SetBackgroundResource(Resource.Drawable.biale);
            RePaint();
            czy_bialy_gracz = true; // gre zaczyna bialy gracz
            czy_pierwsze_klikniecie = true;
        }

        private void RePaint()
        // metoda odswiezajaca widok na podstawie zmieniajacych sie danych po ruchach graczy
        {
            Pionek pionek;
            for (int i_wiersz = 0; i_wiersz < 8; i_wiersz++)
                for (int i_kolumna = 0; i_kolumna < 8; i_kolumna++)
                {
                    pionek = szachownica.szachownica[i_wiersz, i_kolumna];
                    // jesli pole jest czarne
                    if ((i_wiersz + i_kolumna) % 2 == 0)
                    {
                        // jesli pole jest puste
                        if (pionek == null)
                                plansza[i_wiersz, i_kolumna].SetBackgroundResource(Resource.Drawable.czarne);
                        else
                        // jesli istnieje pionek
                        {
                            if (pionek.Czy_Dama)
                            {
                                if (pionek.Czy_Bialy())
                                    plansza[i_wiersz, i_kolumna].SetBackgroundResource(Resource.Drawable.czarne_biale_dama);
                                else
                                    plansza[i_wiersz, i_kolumna].SetBackgroundResource(Resource.Drawable.czarne_czarne_dama);
                            }
                            else
                            {
                                if (pionek.Czy_Bialy())
                                    plansza[i_wiersz, i_kolumna].SetBackgroundResource(Resource.Drawable.czarne_biale);
                                else
                                    plansza[i_wiersz, i_kolumna].SetBackgroundResource(Resource.Drawable.czarne_czarne);
                            }
                        }
                    }
                }
        }
        private void OnClicked(int wiersz, int kolumna)
        // metoda dla kazdego pola
        {
            // jesli to pierwsze klikniecie to zapamietujemy jakim pionkiem gracz chce sie ruszyc, o ile to jego pionek
            if(czy_pierwsze_klikniecie)
            {
                if (szachownica.szachownica[wiersz, kolumna] == null)
                    return;
                // jesli pionek nalezy do aktywnego gracza
                if(szachownica.szachownica[wiersz, kolumna].Czy_Bialy() == czy_bialy_gracz)
                {
                        pozycja.Wiersz = wiersz;
                        pozycja.Kolumna = kolumna;
                        czy_pierwsze_klikniecie = false;
                        Pionek p = szachownica.szachownica[wiersz, kolumna];
                        if (p.Czy_Dama)
                            if (p.Czy_Bialy())
                                plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_biale_dama_zazn);
                            else
                                plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_czarne_dama_zazn);
                        else
                            if (p.Czy_Bialy())
                                plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_biale_zazn);
                            else
                                plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_czarne_zazn);
                }
            }
            // jesli to drugie klikniecie
            else 
            {
                pozycja_dokad.Wiersz = wiersz;
                pozycja_dokad.Kolumna = kolumna;
                // jesli ruch jest prawidlowy to zostaje wykonana zmiana w szachownicy, dzieki czemu mozemy odswiezyc widok
                if (szachownica.Ruch(pozycja, pozycja_dokad))
                {
                    // zostal wykonany ruch, zostaje odswiezona plansza
                    RePaint();
                    // sprawdzenie czy koniec gry
                    if (szachownica.Czy_Wygraly_Biale || szachownica.Czy_Wygraly_Czarne)
                    {
                        if (szachownica.Czy_Wygraly_Biale)
                            toast = Toast.MakeText(this, String.Format("Wygraly Biale"), ToastLength.Short);
                        else
                            toast = Toast.MakeText(this, String.Format("Wygraly Czarne"), ToastLength.Short);
                        toast.Show();
                    }
                    // sprawdzamy czy zostalo wykonane bicie
                    if (szachownica.czy_wykonano_bicie)
                    {
                        // czy jest mozliwosc kolejnego bicia, jesli nie to zmiana gracza
                        if (!szachownica.Czy_Jest_Mozliwe_Kolejne_Bicie(pozycja_dokad))
                        {             
                            czy_bialy_gracz = !czy_bialy_gracz;
                            szachownica.czy_wykonano_bicie = false;
                        }
                    }
                    // jesli nie bylo bicia a ruch byl udany to nastepuje zmiana gracza
                    else
                    {
                        czy_bialy_gracz = !czy_bialy_gracz;
                    }
                }
                // jesli ruch nie byl poprawny odznaczamy wszystkie zaznaczenia
                else
                {
                    wiersz = pozycja.Wiersz;
                    kolumna = pozycja.Kolumna;
                    Pionek p = szachownica.szachownica[wiersz, kolumna];
                    if (p.Czy_Dama)
                        if (p.Czy_Bialy())
                            plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_biale_dama);
                        else
                            plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_czarne_dama);
                    else
                        if (p.Czy_Bialy())
                            plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_biale);
                        else
                            plansza[wiersz, kolumna].SetBackgroundResource(Resource.Drawable.czarne_czarne);
                }
                // jesli ruch byl prawidlowy kolejny ruch gracza jest jego pierwszym ruchem
                // jesli ruch byl nieprawidlowy, gracz musi zaznaczyc jeszcze raz pierwszy pionek
                czy_pierwsze_klikniecie = true;
            }
        }
    }
}