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

namespace Warcaby
{
    public class Pozycja
    {
        private int _kolumna, _wiersz;
        public int Wiersz
        {
            get
            {
                return _wiersz;
            }
            set
            {
                if (CzyPoprawnePole(value))
                    _wiersz = value;
            }
        }

        public int Kolumna
        {
            get
            {
                return _kolumna;
            }

            set
            {
                if (CzyPoprawnePole(value))
                    _kolumna = value;
            }
        }

        public Pozycja(int wiersz, int kolumna)
        {
            if (CzyPoprawnePole(wiersz) && CzyPoprawnePole(kolumna))
            {
                this._wiersz = wiersz;
                this._kolumna = kolumna;
            }
        }
        private bool CzyPoprawnePole(int k)
        {
            if (k >= 0 && k < 8)
                return true;
            else
                return false;
        }
    }
}