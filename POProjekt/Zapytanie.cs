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

        private List<object> objects = new List<object>();
        public IList<object> Objects => objects.AsReadOnly();

        public Zapytanie()
        {
            objects.Add(Osoba);
            objects.Add(Firma);
            objects.Add(Bank);
            objects.Add(Karta);
            objects.Add(Data);
            objects.Add(Kwota);
        }

        public Zapytanie(string pytanie, List<object> obj)
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

            objects = obj;
        }
    }
}
