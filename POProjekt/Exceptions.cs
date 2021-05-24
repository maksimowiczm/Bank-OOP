using System;

namespace POProjekt
{
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
    public class KontoIstnieje : KontoException
    {
        public readonly object gdzie;
        public KontoIstnieje(Konto konto, object gdzie) : base(konto)
        {
            this.gdzie = gdzie;
        }
    }
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
    public class KartaNieIstnieje : KartaException
    {
        public readonly object gdzie;

        public KartaNieIstnieje(Karta karta, object gdzie) : base(karta)
        {
            this.gdzie = gdzie;
        }
    }

    public class KwotaException : Exception
    {
        public readonly decimal kwota;

        public KwotaException(decimal kwota)
        {
            this.kwota = kwota;
        }
    }

    public class WyplacException : Exception
    {
        public readonly decimal kwota;

        public WyplacException(decimal kwota)
        {
            this.kwota = kwota;
        }
    }

    public class UjemnyKredyt : Exception
    {
        public readonly decimal kredyt;

        public UjemnyKredyt(decimal kredyt)
        {
            this.kredyt = kredyt;
        }
    }
}
