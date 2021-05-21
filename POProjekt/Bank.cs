using Newtonsoft.Json; //do zapisu na dysk
using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Bank
    {
        public readonly string Nazwa;
        private List<Karta> karty;
        private List<Konto> konta;
        public List<Karta> Karty => new List<Karta>(karty);
        public List<Konto> Konta => new List<Konto>(konta);
        
        [JsonConstructor]
        public Bank(string nazwa, List<Karta> karty, List<Konto> konta)
        {
            this.Nazwa = nazwa;
            this.karty = karty;
            this.konta = konta;
        }
        public Bank(string nazwa) : this(nazwa, new List<Karta>(), new List<Konto>()) { }
        
        private int mojaKarta(Karta karta) => karty.IndexOf(karta);
        public Karta StworzKarteDebetowa(Klient klient)
        {
            throw new NotImplementedException();
        }
        public Karta StworzKarteKredytowa(Klient klient, decimal kredyt)
        {
            throw new NotImplementedException();
        }
        public bool UsunKarte(Karta karta)
        {
            if (mojaKarta(karta) < 0) return false;
            karty.Remove(karta);
            return true;
        }

        public bool RealizujTransakcje(Karta karta, decimal kwota)
        {
            throw new NotImplementedException();
        }
    }
}
