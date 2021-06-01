using POProjekt;
using System;

namespace POInterfejs
{
    public static class OsobaWidok
    {
        private static readonly string[] zarzadzanieOsobami =
        {
            "Dodaj osobę",
            "Wyświetl osobę",
            "Powrót",
        };

        public static void ZarzadzajOsoba(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieOsobami.Length)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzanieOsobami);
                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                Console.Clear();
                switch (wybor)
                {
                    case 1:
                        DodajOsobe(centrum);
                        break;
                    case 2:
                        WyswietlOsobe(centrum);
                        break;
                }
            }
        }

        private static void ZarzadzajKartmi(Centrum centrum, Osoba osoba)
        {
            string[] zarzadzajKartmi =
            {
                "Zarządzaj istniejącymi",
                "Stwórz nową",
                "Powrót",
            };


            var wybor = 0;
            while (wybor != zarzadzajKartmi.Length)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzajKartmi);
                if (!int.TryParse(Console.ReadLine(), out wybor))
                    continue;

                switch (wybor)
                {
                    case 1:
                        Console.Clear();
                        KartaWidok.ZarzadzajKarta(KartaWidok.WybierzKarte(osoba));
                        break;
                    case 2:
                        KartaWidok.DodajKarta(centrum, osoba);
                        break;
                }
            }
        }

        private static void ZarzadzajKontami(Centrum centrum, Osoba osoba)
        {
            string[] zarzadzajKontami =
            {
                "Zarządzaj istniejącym",
                "Stwórz nowe",
                "Powrót",
            };


            var wybor = 0;
            while (wybor != zarzadzajKontami.Length)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzajKontami);
                if (!int.TryParse(Console.ReadLine(), out wybor))
                    continue;

                switch (wybor)
                {
                    case 1:
                        Console.Clear();
                        KontoWidok.Zarzadzaj(KontoWidok.WybierzKonto(osoba));
                        break;
                    case 2:
                        KontoWidok.DodajKonto(centrum, osoba);
                        break;
                }
            }
        }

        private static readonly string osobaHeader = $"Numer{"Imie",25}{"Nazwisko",25}{"Ilość kart",25}{"Ilość kont",25}";

        public static void DodajOsobe(Centrum centrum)
        {
            Console.WriteLine("Podaj imię");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko");
            var nazwisko = Console.ReadLine();
            centrum.DodajOsobe(new Osoba(imie, nazwisko));
        }

        public static void WyswietlOsoby(Centrum centrum)
        {
            Console.WriteLine(osobaHeader);
            var i = 1;
            foreach (var osoba in centrum.Osoby)
                Console.WriteLine($"{i++,3}. {osoba.ToString("f")}");
        }

        public static Osoba WybierzOsobe(Centrum centrum)
        {
            var nic = 0;
            return WybierzOsobe(centrum, ref nic);
        }

        public static Osoba WybierzOsobe(Centrum centrum, ref int wybor)
        {
            WyswietlOsoby(centrum);
            while (wybor < 1 || wybor > centrum.Osoby.Count)
                int.TryParse(Console.ReadLine(), out wybor);
            return centrum.Osoby[wybor - 1];
        }

        public static void WyswietlOsobe(Centrum centrum)
        {
            if (!MenuWidok.WeryfikujCentrum(centrum))
                return;
            string[] opcje =
            {
                "Zarządzaj kontami",
                "Zarządzaj kartami",
                "Powrót",
            };

            Console.WriteLine("Podaj numer osoby, którą chcesz wyświetlić");

            var wybor = 0;
            var osoba = WybierzOsobe(centrum, ref wybor);
            var srodek = 0;

            while (srodek != opcje.Length)
            {
                Console.Clear();
                Console.WriteLine("Wybrana Osoba");
                Console.WriteLine(osobaHeader);
                Console.WriteLine($"{wybor,3}. {osoba.ToString("f")}");

                Console.WriteLine("\nKarty");
                Console.WriteLine(KartaWidok.KartaHeader);
                foreach (var karta in osoba.Karty)
                {
                    var typ = karta.GetType() == typeof(Kredytowa) ? "kredytowa" : "debetowa";
                    Console.WriteLine($"{typ,9} {karta}");
                }

                Console.WriteLine("\nKonta");
                Console.WriteLine($"{"Bank",10} {"Saldo",10}");
                foreach (var konto in osoba.Konta)
                {
                    var bank = konto.Bank;
                    Console.WriteLine($"{bank.ToString("s"),10} {konto.ToString("s"),10}");
                }


                Console.WriteLine('\n');
                Widok.Wyswietl(opcje);

                if (!int.TryParse(Console.ReadLine(), out srodek))
                    continue;
                switch (srodek)
                {
                    case 1:
                        ZarzadzajKontami(centrum, osoba);
                        break;
                    case 2:
                        ZarzadzajKartmi(centrum, osoba);
                        break;
                }
                Console.Clear();
            }
        }
    }
}