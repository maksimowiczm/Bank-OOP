using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Centrum
    {
        private List<Transakcja> transakcje;
        private List<Osoba> osoby;
        private List<Firma> firmy;
        private List<Bank> banki;
        public List<Transakcja> Transakcje => new List<Transakcja>(transakcje);
        public List<Osoba> Osoby => new List<Osoba>(osoby);
        public List<Firma> Firmy => new List<Firma>(firmy);
        public List<Bank> Banki => new List<Bank>(banki);

        public Centrum() : this(new(), new(), new(), new())
        {
        }
        public Centrum(List<Transakcja> transakcje, List<Osoba> osoby, List<Firma> firmy, List<Bank> banki)
        {
            this.transakcje = transakcje;
            this.osoby = osoby;
            this.firmy = firmy;
            this.banki = banki;
        }

        public bool AutoryzujTransakcje(Firma firma, Karta karta, decimal kwota)
        {
            var bank = karta.Bank;
            var sukces = bank.RealizujTransakcje(karta, kwota);
            var transakcja = new Transakcja(DateTime.Now, sukces, bank, firma, karta.Klient, karta, kwota);
            transakcje.Add(transakcja);
            return sukces;
        }

        public void DodajOsobe(Osoba osoba) => osoby.Add(osoba);
        public void DodajFirme(Firma firma) => firmy.Add(firma);
        public void DodajBank(Bank bank) => banki.Add(bank);

        public void Zapisz(string uri)
        {
            throw new NotImplementedException();
        }

    }
}
