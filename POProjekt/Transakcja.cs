using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public class Transakcja
    {
        private static int ilosc = 0;
        public readonly int Id;
        public readonly DateTime Data;
        public readonly bool Sukces;
        public readonly Bank Bank;
        public readonly Firma Firma;
        public readonly Osoba Osoba;
        public readonly Karta Karta;
        public readonly decimal Kwota;

        [JsonConstructor]
        public Transakcja(int id, DateTime data, bool sukces, Bank bank, Firma firma, Osoba osoba, Karta karta, decimal kwota)
        {
            Id = id;
            ilosc++;
            Data = data;
            Sukces = sukces;
            Bank = bank;
            Firma = firma;
            Osoba = osoba;
            Karta = karta;
            Kwota = kwota;
        }
        public Transakcja(DateTime data, bool sukces, Bank bank, Firma firma, Osoba osoba, Karta karta, decimal kwota)
            : this(ilosc, data, sukces, bank, firma, osoba, karta, kwota) { }

        public class TransakcjaJson
        {
            public int id;
            public string data;
            public bool sukces;
            public decimal kwota;
            public string bankOsoby;
            public string osoba;
            public string bankFirmy;
            public string firma;
            public int karta;

            public TransakcjaJson(int id, string data, bool sukces, decimal kwota, Bank bank, Osoba osoba, Firma firma, Karta karta)
            {
                this.id = id;
                this.data = data;
                this.sukces = sukces;
                this.kwota = kwota;
                this.bankOsoby = karta.Bank.ToString();
                this.osoba = osoba.ToString();
                bankFirmy = firma.Konta[Firma.DomyslneKonto].Bank.ToString();
                this.firma = firma.ToString();
                this.karta = karta.Numer;
            }
        }
        public TransakcjaJson Json() => new(Id, Data.ToString("G"), Sukces, Kwota, Bank, Osoba, Firma, Karta);
        public override string ToString()
        {
            var TrasakcjaJson = new TransakcjaJson(Id, Data.ToString("G"), Sukces, Kwota, Bank, Osoba, Firma, Karta);
            return JsonConvert.SerializeObject(TrasakcjaJson, Formatting.Indented);
        }
    }
}
