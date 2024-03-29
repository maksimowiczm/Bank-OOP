﻿using POProjekt;
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

        public static readonly string KartaHeader = $"{"Typ karty",9} {"Numer",10} {"Imie",10} {"Nazwisko",15} {"Saldo",10} Maksymalny kredyt";
        public static readonly string KartaHeaderS = $"{"Numer",10} {"Imie",10} {"Nazwisko",15} {"Saldo",10} Maksymalny kredyt";

        public static void Zarzadzaj(Karta karta)
        {
            var wybor = 0;
            while (wybor != zarzadzajKarta.Length)
            {
                Console.WriteLine(KartaHeaderS);
                Console.WriteLine(karta);
                Widok.WyswietlIndex(zarzadzajKarta);
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
            Widok.WyswietlIndex(new[] { "Kredytowa", "Debetowa", });
            var wybor = 0;
            while (wybor is < 1 or > 2)
                int.TryParse(Console.ReadLine(), out wybor);

            var bank = new Bank("");
            switch (wybor)
            {
                case 1:
                    Console.WriteLine("Wybierz bank dla karty");
                    bank = BankWidok.WybierzBank(centrum);
                    Console.WriteLine("Podaj maksymalny kredyt");
                    decimal kredyt;
                    while (!decimal.TryParse(Console.ReadLine(), out kredyt)) { }

                    bank.StworzKarteKredytowa(osoba, kredyt);
                    break;
                case 2:
                    Console.WriteLine("Najpierw stwórz konto");
                    var konto = KontoWidok.DodajKonto(centrum, osoba, ref bank);
                    bank.StworzKarteDebetowa(osoba, konto);
                    break;
            }
            Console.WriteLine("Karta stworzona");
            Console.Read();
            Console.Read();
        }

        public static Karta WybierzKarte(Osoba osoba)
        {
            Console.WriteLine($"   {KartaHeader}");

            Widok.WyswietlIndex(osoba.Karty.Select(karta => $"{(karta.GetType() == typeof(Debetowa) ? "Debetowa" : "Kredytowa"),9} {karta}").ToList());
            var wybor = 0;
            while (wybor < 1 || wybor > osoba.Karty.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return osoba.Karty[wybor - 1];
        }
    }
}