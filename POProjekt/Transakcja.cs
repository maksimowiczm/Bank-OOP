using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public class Transakcja
    {
        private static int ilosc;
        public readonly int Id;
        public readonly DateTime Data;
        public readonly bool Sukces;
        public readonly decimal Kwota;
        public readonly string BankOsoby;
        public readonly string Osoba;
        public readonly string BankFirmy;
        public readonly string Firma;
        public readonly int Karta;

        [JsonConstructor]
        public Transakcja(int id, DateTime data, bool sukces, decimal kwota, string bankOsoby, string osoba, string bankFirmy, string firma, int karta)
        {
            Id = id;
            ilosc++;
            Data = data;
            Sukces = sukces;
            Kwota = kwota;
            BankOsoby = bankOsoby;
            Osoba = osoba;
            BankFirmy = bankFirmy;
            Firma = firma;
            Karta = karta;
        }

        public Transakcja(DateTime data, bool sukces, decimal kwota, Firma firma, Osoba osoba, Karta karta)
        {
            Id = ilosc++;
            Data = data;
            Sukces = sukces;
            Kwota = kwota;
            BankOsoby = karta.Bank.Nazwa;
            Osoba = osoba.ToString("j");
            BankFirmy = firma.Konta[0].Bank.Nazwa;
            Firma = firma.Nazwa;
            Karta = karta.Numer;
        }

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
