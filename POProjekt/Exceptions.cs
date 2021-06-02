using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class ZapytanieException : Exception
    {
        public readonly string Pytanie;
        public readonly object Cos;

        public ZapytanieException(string pytanie)
        {
            Pytanie = pytanie;
        }
        public ZapytanieException(Zapytanie zapytanie) : this(zapytanie.Pytanie)
        {
            Pytanie = zapytanie.Pytanie;
            Cos = zapytanie;
        }
    }

    public class WczytwanieZapisException : Exception
    {
        public readonly string Sciezka;

        public WczytwanieZapisException(string sciezka)
        {
            Sciezka = sciezka;
        }
    }

    public abstract class DeserializacjaException : WczytwanieZapisException
    {
        protected DeserializacjaException(string sciezka) : base(sciezka) { }
    }
    public class DeserializacjaException<T> : DeserializacjaException
    {
        public readonly object obj;
        private Dictionary<int, T> dic;
        public IReadOnlyDictionary<int, T> Dic => dic;
        public readonly Exception E;

        public DeserializacjaException(string sciezka, object obj, Dictionary<int, T> dic, Exception e) : base(sciezka)
        {
            this.obj = obj;
            this.dic = dic;
            this.E = e;
        }
    }

    /// <summary> Coś nie działa z firma, w wiadomości jest co. </summary>
    public class FirmaException : Exception
    {
        public readonly Firma Firma;

        public FirmaException(Firma firma, string message) : base(message)
        {
            Firma = firma;
        }
    }

    public class KontoException : Exception
    {
        public readonly Konto Konto;
        public KontoException(Konto konto)
        {
            Konto = konto;
        }
    }

    /// <summary> Podane konto jest już na liście kont w ty obiekcie </summary>
    public class KontoIstnieje : KontoException
    {
        public readonly object gdzie;
        public KontoIstnieje(Konto konto, object gdzie) : base(konto)
        {
            this.gdzie = gdzie;
        }
    }

    /// <summary> Podane konto nie jest na liście kont w ty obiekcie </summary>
    public class KontoNieIstnieje : KontoIstnieje
    {
        public KontoNieIstnieje(Konto konto, object gdzie) : base(konto, gdzie) { }
    }

    public class KartaException : Exception
    {
        public readonly Karta Karta;

        public KartaException(Karta karta)
        {
            Karta = karta;
        }
    }

    /// <summary> Podana karta jest już na liście kont w ty obiekcie. </summary>
    public class KartaNieIstnieje : KartaException
    {
        public readonly object gdzie;

        public KartaNieIstnieje(Karta karta, object gdzie) : base(karta)
        {
            this.gdzie = gdzie;
        }
    }

    /// <summary> Podana karta nie jest na liście kont w ty obiekcie. </summary>
    public class KwotaException : Exception
    {
        public readonly decimal kwota;

        public KwotaException(decimal kwota)
        {
            this.kwota = kwota;
        }
    }

    /// <summary> Próba wpłacenia nie dodatniej kwoty na konto lub kartę. </summary>
    public class WyplacException : Exception
    {
        public readonly decimal kwota;

        public WyplacException(decimal kwota)
        {
            this.kwota = kwota;
        }
    }

    /// <summary> Podanie złego kredytu podczas tworzenia karty kredytowej </summary>
    public class UjemnyKredyt : Exception
    {
        public readonly decimal kredyt;

        public UjemnyKredyt(decimal kredyt)
        {
            this.kredyt = kredyt;
        }
    }
}
