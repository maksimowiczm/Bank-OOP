using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public abstract class Karta
    {
        private static int ilosc;

        public readonly Bank Bank;
        public readonly Osoba Osoba;
        public readonly int Numer;

        [JsonConstructor]
        protected Karta(Bank bank, Osoba osoba, int numer)
        {
            Bank = bank;
            Osoba = osoba;
            Numer = numer;
            ilosc++;
        }
        protected Karta(Bank bank, Osoba osoba) : this(bank, osoba, ilosc) { }

        protected bool ZweryfikujKwote(decimal kwota)
        {
            if (kwota <= 0)
                throw new Exception("Niedodatnia kwota");
            return true;
        }
        public abstract bool Wyplac(decimal kwota);
    }
}
