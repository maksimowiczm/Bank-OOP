using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Zapytanie
    {
        public string Pytanie { get; init; }
        public Osoba Osoba { get; init; }
        public Firma Firma { get; init; }
        public Bank Bank { get; init; }
        public Karta Karta { get; init; }
        public DateTime? Data { get; init; }
        public decimal? Kwota { get; init; }

        public Zapytanie() : this(null, null, null, null, null, null, null) { }
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

        public Zapytanie(string pytanie, IEnumerable<object> obj)
        {
            Pytanie = pytanie;
            foreach (var o in obj)
            {
                switch (o)
                {
                    case Osoba osoba:
                        Osoba = osoba;
                        break;
                    case Firma firma:
                        Firma = firma;
                        break;
                    case Bank bank:
                        Bank = bank;
                        break;
                    case Karta karta:
                        Karta = karta;
                        break;
                    case DateTime date:
                        Data = date;
                        break;
                    case decimal dec:
                        Kwota = dec;
                        break;
                }
            }
        }
    }
}
