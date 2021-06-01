namespace POProjekt
{
    public abstract class Karta
    {
        private static int iloscKart;

        public readonly Bank Bank;
        public readonly Osoba Osoba;
        public readonly int Numer;
        public abstract decimal Saldo { get; }

        protected Karta(Bank bank, Osoba osoba, int numer)
        {
            Bank = bank;
            Osoba = osoba;
            Numer = numer;
            iloscKart++;
        }

        protected Karta(Bank bank, Osoba osoba) : this(bank, osoba, iloscKart) { }

        /// <summary> Sprawdza czy podana kwota jest dodatnia. </summary>
        protected bool ZweryfikujKwote(decimal kwota)
        {
            if (kwota <= 0)
                throw new KwotaException(kwota);
            return true;
        }

        public abstract void Wplac(decimal kwota);

        public abstract bool Wyplac(decimal kwota);

        public abstract void Zapisz(string dir);

        public override bool Equals(object obj) => obj is Karta druga && druga.Numer == Numer;

        public override string ToString() => $"{Numer,10} {Osoba.ToString("s"),26}";
    }
}
