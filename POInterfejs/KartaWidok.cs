using POProjekt;
using System;
using System.Linq;

namespace POInterfejs
{
    public static class KartaWidok
    {
        private static readonly string[] zarzadzajKarta =
        {
            "Wpłać",
            "Wypłać",
            "Powrót",
        };

        public static readonly string KartaHeader =
            $"{"Typ karty",9} {"Numer",10} {"Imie",10} {"Nazwisko",15} {"Saldo",10} Maksymalny kredyt";

        public static void Zarzadzaj(Karta karta)
        {
            var wybor = 0;
            while (wybor != zarzadzajKarta.Length)
            {
                Widok.Wyswietl(zarzadzajKarta);
                if (!int.TryParse(Console.ReadLine(), out wybor))
                    continue;

                Console.Clear();
                decimal kwota;
                switch (wybor)
                {
                    case 1:
                        Console.WriteLine("Podaj kwotę do wpłacenia");
                        while (!decimal.TryParse(Console.ReadLine(), out kwota)) { }

                        karta.Wplac(kwota);
                        Console.WriteLine($"Wpłacono. Aktualne saldo {karta.Saldo}");
                        break;
                    case 2:
                        Console.WriteLine("Podaj kwotę do wypłacenia");
                        while (!decimal.TryParse(Console.ReadLine(), out kwota)) { }

                        if (!karta.Wyplac(kwota))
                            Console.WriteLine("Kwota jest zbyt duża żeby ją wypłacić");
                        break;
                }
            }
        }

        public static void DodajKarta(Centrum centrum, Osoba osoba)
        {
            Console.Clear();
            Console.WriteLine("Wybierz typ karty");
            Widok.Wyswietl(new[] { "Kredytowa", "Debetowa", });
            var wybor = 0;
            while (wybor is < 1 or > 2)
                int.TryParse(Console.ReadLine(), out wybor);

            Console.WriteLine("Wybierz bank dla karty");
            var bank = BankWidok.WybierzBank(centrum);
            switch (wybor)
            {
                case 1:
                    Console.WriteLine("Podaj maksymalny kredyt");
                    decimal kredyt;
                    while (!decimal.TryParse(Console.ReadLine(), out kredyt)) { }

                    bank.StworzKarteKredytowa(osoba, kredyt);
                    break;
                case 2:
                    Console.WriteLine("Wybierz konto karty");
                    var konto = KontoWidok.WybierzKonto(osoba);
                    bank.StworzKarteDebetowa(osoba, konto);
                    break;
            }

            Console.WriteLine("Karta stworzona");
            Console.Read();
        }

        public static Karta WybierzKarte(Osoba osoba)
        {
            Console.WriteLine($"   {KartaHeader}");

            Widok.Wyswietl(osoba.Karty.Select(karta => $"{(karta.GetType() == typeof(Debetowa) ? "debetowa" : "kredytowa"),9} {karta}").ToList());
            var wybor = 0;
            while (wybor < 1 || wybor > osoba.Karty.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return osoba.Karty[wybor - 1];
        }
    }
}