using System;

namespace POProjekt
{
    public class Zapytanie
    {
        public string Pytanie;
        public readonly Osoba Osoba;
        public readonly Firma Firma;
        public readonly Bank Bank;
        public readonly Karta Karta;
        public readonly DateTime? Data;
        public readonly decimal? Kwota;

        public Zapytanie(string pytanie, Osoba osoba, Firma firma, Bank bank, Karta karta, DateTime? data, decimal? kwota)
        {
            Pytanie = pytanie;
            Osoba = osoba;
            Firma = firma;
            Bank = bank;
            Karta = karta;
            Data = data;
            Kwota = kwota;
        }
    }
}
