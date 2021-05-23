using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Centrum
    {
        private readonly List<Transakcja> transakcje;
        private readonly List<Osoba> osoby;
        private readonly List<Firma> firmy;
        private readonly List<Bank> banki;
        public IList<Transakcja> Transakcje => transakcje.AsReadOnly();
        public IList<Osoba> Osoby => osoby.AsReadOnly();
        public IList<Firma> Firmy => firmy.AsReadOnly();
        public IList<Bank> Banki => banki.AsReadOnly();

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
            var transakcja = new Transakcja(DateTime.Now, sukces, bank, firma, karta.Osoba, karta, kwota);
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
