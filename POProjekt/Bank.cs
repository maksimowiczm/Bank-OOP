using Newtonsoft.Json; //do zapisu na dysk
using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Bank
    {
        public readonly string Nazwa;
        private readonly List<Karta> karty;
        private readonly List<Konto> konta;
        public IList<Karta> Karty => karty.AsReadOnly();
        public IList<Konto> Konta => konta.AsReadOnly();

        [JsonConstructor]
        public Bank(string nazwa, List<Karta> karty, List<Konto> konta)
        {
            this.Nazwa = nazwa;
            this.karty = karty;
            this.konta = konta;
        }
        public Bank(string nazwa) : this(nazwa, new List<Karta>(), new List<Konto>()) { }

        private int mojaKarta(Karta karta) => karty.IndexOf(karta);
        public Karta StworzKarteDebetowa(Osoba osoba)
        {
            throw new NotImplementedException();
        }
        public Karta StworzKarteKredytowa(Osoba osoba, decimal kredyt)
        {
            var karta = new Kredytowa(this, osoba, kredyt);
            karty.Add(karta);
            return karta;
        }
        public bool UsunKarte(Karta karta)
        {
            if (mojaKarta(karta) < 0) return false;
            karty.Remove(karta);
            return true;
        }

        public bool RealizujTransakcje(Karta karta, decimal kwota)
        {
            if (mojaKarta(karta) == -1)
                return false;
            return karta.Wyplac(kwota);
        }
    }
}
