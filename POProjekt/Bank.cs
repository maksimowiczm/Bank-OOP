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

        private bool mojeKonto(Konto konto) => konta.Contains(konto);
        public Konto StworzKonto(Klient klient)
        {
            var konto = new Konto(this, klient);
            konta.Add(konto);
            return konto;
        }
        public bool UsunKonto(Konto konto)
        {
            if (!konta.Contains(konto)) return false;

            konta.Remove(konto);
            return true;
        }

        public Karta StworzKarteDebetowa(Osoba osoba, Konto konto)
        {
            if (!mojeKonto(konto))
                throw new Exception("Nie moje konto");

            var karta = new Debetowa(this, osoba, konto);
            karty.Add(karta);
            return karta;
        }
        public Karta StworzKarteKredytowa(Osoba osoba, decimal kredyt)
        {
            var karta = new Kredytowa(this, osoba, kredyt);
            karty.Add(karta);
            return karta;
        }
        private bool mojaKarta(Karta karta) => karty.Contains(karta);
        public bool UsunKarte(Karta karta)
        {
            if (!mojaKarta(karta)) return false;
            karty.Remove(karta);
            return true;
        }

        public bool RealizujTransakcje(Karta karta, decimal kwota) => mojaKarta(karta) && karta.Wyplac(kwota);
    }
}
