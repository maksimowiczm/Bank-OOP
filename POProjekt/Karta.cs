using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public abstract class Karta
    {
        private static int ilosc;

        public readonly Bank Bank;
        public readonly Klient Klient;
        public readonly int Numer;

        [JsonConstructor]
        protected Karta(Bank bank, Klient klient, int numer)
        {
            Bank = bank;
            Klient = klient;
            Numer = numer;
            ilosc++;
        }
        protected Karta(Bank bank, Klient klient) : this(bank, klient, ilosc) { }

        protected bool ZweryfikujKwote(decimal kwota)
        {
            if (kwota <= 0)
                throw new Exception("Niedodatnia kwota");
            return true;
        }
        public abstract bool Wyplac(decimal kwota);
    }
}
