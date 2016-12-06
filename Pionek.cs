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

public class Pionek
{
    private Pozycja _pozycja;
    private bool _czy_bialy;
    private bool _czy_dama;
    
    public Pionek(bool czy_bialy, Pozycja pozycja, bool czy_dama)
	{
        this._czy_bialy = czy_bialy;
        this._pozycja = pozycja;
        this._czy_dama = czy_dama;
	}

    public bool Czy_Bialy()
    {
        return _czy_bialy;
    }
    // Wlasciwosci
    public Pozycja Pozycja
    {
        get
        {
            return _pozycja;
        }
        set
        {
            _pozycja = value;
        }
    }

    public bool Czy_Dama
    {
        get
        {
            return _czy_dama;
        }
        set
        {
            _czy_dama = value;
        }
    }


}
