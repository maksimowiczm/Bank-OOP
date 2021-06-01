using POProjekt;
using System;
using System.Linq;

namespace POInterfejs
{
    public static class KontoWidok
    {
        public static readonly string KontoHeader = $"{"Saldo",10} {"Bank",15}";

        public static void Zarzadzaj(Konto konto)
        {
            string[] zarzadzajKontem =
            {
                "Wpłać",
                "Wypłać",
                "Powrót",
            };
            Console.WriteLine("Wybierz konto");

            var wybor = 0;
            while (wybor != zarzadzajKontem.Length)
            {
                Widok.Wyswietl(zarzadzajKontem);
                if (!int.TryParse(Console.ReadLine(), out wybor))
                    continue;

                Console.Clear();
                decimal kwota;
                switch (wybor)
                {
                    case 1:
                        Console.WriteLine("Podaj kwotę do wpłacenia");
                        while (!decimal.TryParse(Console.ReadLine(), out kwota)) { }

                        konto.Wplac(kwota);
                        Console.WriteLine($"Wpłacono. Aktualne saldo {konto.Saldo}");
                        break;
                    case 2:
                        Console.WriteLine("Podaj kwotę do wypłacenia");
                        while (!decimal.TryParse(Console.ReadLine(), out kwota)) { }

                        if (!konto.Wyplac(kwota))
                            Console.WriteLine("Kwota jest zbyt duża żeby ją wypłacić");
                        break;
                }
            }
        }

        public static void DodajKonto(Centrum centrum, Klient klient)
        {
            Console.Clear();
            Console.WriteLine("Wybierz bank dla nowego konta");
            BankWidok.WybierzBank(centrum).StworzKonto(klient);
            Console.WriteLine("Konto stworzone");
            Console.Read();
        }

        public static Konto WybierzKonto(Osoba osoba)
        {
            Console.WriteLine($"   {KontoHeader}");

            Widok.Wyswietl(osoba.Konta.Select(konto => konto.ToString("b")).ToList());
            var wybor = 0;
            while (wybor < 1 || wybor > osoba.Karty.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return osoba.Konta[wybor - 1];
        }
    }
}