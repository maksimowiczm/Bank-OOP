using Newtonsoft.Json; //do zapisu na dysk
using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Bank
    {
        private static int ilosc;
        private static List<Bank> banki = new List<Bank>();
        public readonly int Id;
        public readonly string Nazwa;
        private List<Karta> karty;
        private List<Konto> konta;

        public static Bank GetBank(int id) => banki.Find(b => b.Id == id);
        [JsonConstructor]
        public Bank(string nazwa, int id, List<Karta> karty, List<Konto> konta)
        {
            if (id < 0) throw new Exception("Id ujemne");
            this.Nazwa = nazwa;
            this.Id = id;
            this.karty = karty;
            this.konta = konta;
            ilosc++;
            banki.Add(this);
        }
        public Bank(string nazwa) : this(nazwa, ilosc, new List<Karta>(), new List<Konto>()) { }

        public List<Karta> Karty { get => karty; }  //prop
        public List<Konto> Konta { get => konta; }

        private int mojaKarta(Karta karta) => karty.IndexOf(karta);

        public Konto StworzKonto(int idFirmy) => StworzKonto(idFirmy, 0);

        public Konto StworzKonto(int idFirmy, decimal saldo)
        {
            if (saldo < 0)
                throw new Exception("Ujemne saldo");
            var konto = new Konto(Id, idFirmy, saldo);
            konta.Add(konto);
            return konto;
        }
        public Karta StworzKarteDebetowa(Klient klient, decimal saldo)
        {
            Karta karta = new Debetowa(this.Id, klient.Id, saldo);
            karty.Add(karta);
            return karta;
        }
        public Karta StworzKarteKredytowa(Klient klient, decimal kredyt, decimal saldo)
        {
            Karta karta = new Kredytowa(this.Id, klient.Id, kredyt, saldo);
            karty.Add(karta);
            return karta;
        }
        public bool UsunKarte(Karta karta)
        {
            if (mojaKarta(karta) < 0) return false;
            karty.Remove(karta);
            return true;
        }
        public bool RealizujTransakcje(Karta karta, decimal kwota) => mojaKarta(karta) > -1 && karta.Wyplac(kwota);
    }
}
