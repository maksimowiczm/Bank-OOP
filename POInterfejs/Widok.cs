using System;
using System.Collections.Generic;
using System.Linq;
using POProjekt;

namespace POInterfejs
{
    public static class Widok
    {
        public static void Wyswietl(IEnumerable<string> lista)
        {
            var i = 1;
            foreach (var s in lista)
                Console.WriteLine($"{i++}. {s}");
        }
    }

    public static class MenuWidok
    {
        private static readonly string[] menu =
        {
            "Zarządzanie bankami",
            "Zarządzanie osobami",
            "Zarządzanie firmami",
            "Zarządzanie trasakcjami",
            "Zrealizuj transakcję",
            "Zapisz",
            "Wyjdz",
        };

        public static void Start(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != menu.Length)
            {
                Console.Clear();
                Wyswietl();

                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                switch (wybor)
                {
                    case 1:
                        BankWidok.ZarzadzajBank(centrum);
                        break;
                    case 2:
                        OsobaWidok.ZarzadzajOsoba(centrum);
                        break;
                    case 3:
                        FirmaWidok.ZarzadzajFirma(centrum);
                        break;
                    case 4:
                        TransakcjaWidok.ZarzadzajTransakcjami(centrum);
                        break;
                    case 5:
                        ZrealizujTransakcje(centrum);
                        break;
                    case 6:
                        Zapisz(centrum);
                        break;
                    case 7:
                        Zapisz(centrum, "autozapis");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public static bool WeryfikujCentrum(Centrum centrum)
        {
            if (centrum.Banki.Count == 0)
            {
                Console.WriteLine("Najpierw stwórz bank");
                Console.Read();
                return false;
            }
            else if (centrum.Osoby.Count == 0)
            {
                Console.WriteLine("Najpierw stwórz osobę");
                Console.Read();
                return false;
            }
            else if (centrum.Firmy.Count == 0)
            {
                Console.WriteLine("Najpierw stwórz firmę");
                Console.Read();
                return false;
            }

            return true;
        }
        public static void ZrealizujTransakcje(Centrum centrum, Firma firma)
        {
            Console.Clear();
            Console.WriteLine("Wybierz osobę do transakcji");
            var osoba = OsobaWidok.WybierzOsobe(centrum);

            Console.Clear();
            Console.WriteLine("Podaj Kwotę transakcji");
            decimal kwota;
            while (!decimal.TryParse(Console.ReadLine(), out kwota)) { }

            Console.Clear();
            Console.WriteLine("Wybierz kartę do zaplaty");

            var karta = KartaWidok.WybierzKarte(osoba);

            Console.Clear();
            Console.WriteLine("Realizuj transakcję");
            Widok.Wyswietl(new string[] { "tak", "nie" });
            var wybor = 0;
            while (wybor < 1 || wybor > 2)
                int.TryParse(Console.ReadLine(), out wybor);

            var sukces = firma.PoprosOAutoryzacje(karta, kwota);
            Console.WriteLine($"Transakcja {(sukces ? "" : "nie")} udana.");
            Console.WriteLine(centrum.Transakcje.Last().ToString());
            Console.Read();
        }
        public static void ZrealizujTransakcje(Centrum centrum)
        {
            if(!WeryfikujCentrum(centrum))
                return;
            Console.Clear();
            Console.WriteLine("Wybierz firmę do transakcji");
            var firma = FirmaWidok.WybierzFirme(centrum);
            ZrealizujTransakcje(centrum, firma);
        }

        public static void Wyswietl() => Widok.Wyswietl(menu);

        public static void Zapisz(Centrum centrum, string nazwa) => centrum.Zapisz(nazwa);

        public static void Zapisz(Centrum centrum)
        {
            Console.Clear();
            Console.WriteLine("Podaj nazwę zapisu:");
            var nazwa = Console.ReadLine();
            Zapisz(centrum, nazwa);
        }
    }

    public static class KartaWidok
    {
        public static readonly string KartaHeader =
            $"{"Typ karty",9} {"Numer",10} {"Imie",10} {"Nazwisko",15} {"Saldo",10} Maksymalny kredyt";

        public static Karta WybierzKarte(Osoba osoba)
        {
            Console.WriteLine($"   {KartaHeader}");

            Widok.Wyswietl(osoba.Karty.Select(karta => $"{(karta.GetType() == typeof(Debetowa) ? "debetowa" : "kredytowa"),9} {karta}").ToList());
            var wybor = 0;
            while (wybor < 1 || wybor > osoba.Karty.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return osoba.Karty[wybor - 1];
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

        private static readonly string[] zarzadzajKarta =
        {
            "Wpłać",
            "Wypłać",
            "Powrót",
        };

        public static void ZarzadzajKarta(Karta karta)
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
    }

    public static class KontoWidok
    {
        public static readonly string KontoHeader = $"{"Saldo",10} {"Bank",15}";

        public static Konto WybierzKonto(Osoba osoba)
        {
            Console.WriteLine($"   {KontoHeader}");

            Widok.Wyswietl(osoba.Konta.Select(konto => konto.ToString("b")).ToList());
            var wybor = 0;
            while (wybor < 1 || wybor > osoba.Karty.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return osoba.Konta[wybor - 1];
        }

        public static void DodajKonto(Centrum centrum, Klient klient)
        {
            Console.Clear();
            Console.WriteLine("Wybierz bank dla nowego konta");
            BankWidok.WybierzBank(centrum).StworzKonto(klient);

            Console.WriteLine("Konto stworzone");
            Console.Read();
        }

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
    }

    public static class BankWidok
    {
        private static readonly string[] zarzadzanieBankami =
        {
            "Dodaj bank",
            "Wyświetl bank",
            "Powrót",
        };

        public static void ZarzadzajBank(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieBankami.Length)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzanieBankami);
                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                Console.Clear();
                switch (wybor)
                {
                    case 1:
                        DodajBank(centrum);
                        break;
                    case 2:
                        WyswietlBank(centrum);
                        break;
                }
            }
        }

        private static readonly string bankHeader = $"Numer{"Nazwa",25} {"Ilość kart",25} {"Ilość kont",25}";

        public static void DodajBank(Centrum centrum)
        {
            Console.WriteLine("Podaj nazwę banku");
            centrum.DodajBank(new Bank(Console.ReadLine()));
        }

        public static void WyswietlBanki(Centrum centrum)
        {
            Console.WriteLine(bankHeader);
            var i = 1;
            foreach (var bank in centrum.Banki)
                Console.WriteLine($"{i++,3}. {bank}");
        }

        public static Bank WybierzBank(Centrum centrum)
        {
            var nic = 0;
            return WybierzBank(centrum, ref nic);
        }
        public static Bank WybierzBank(Centrum centrum, ref int wybor)
        {
            WyswietlBanki(centrum);
            while (wybor < 1 || wybor > centrum.Banki.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return centrum.Banki[wybor - 1];
        }

        public static void WyswietlBank(Centrum centrum)
        {
            if(!MenuWidok.WeryfikujCentrum(centrum))
                return;
            Console.WriteLine("Podaj numer banku, który chcesz wyświetlić");
            var wybor = 0;
            var bank = WybierzBank(centrum, ref wybor);

            Console.Clear();
            Console.WriteLine("Wybrany bank");
            Console.WriteLine(bankHeader);
            Console.WriteLine($"{wybor,3}. {bank}");

            Console.WriteLine("\nKarty");
            Console.WriteLine(KartaWidok.KartaHeader);
            foreach (var karta in bank.Karty)
            {
                var typ = karta.GetType() == typeof(Kredytowa) ? "kredytowa" : "debetowa";
                Console.WriteLine($"{typ,9} {karta}");
            }

            Console.WriteLine("\nKonta");
            Console.WriteLine($"{"Klient",32} {"Saldo",10}");
            foreach (var konto in bank.Konta)
            {
                var klient = konto.Klient.GetType() == typeof(Firma) ? "firma" : "osoba";
                Console.WriteLine($"{klient,5} {konto.ToString("f")}");
            }
        }
    }

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

    public static class FirmaWidok
    {
        private static readonly string[] zarzadzanieFirmami =
        {
            "Dodaj frimę",
            "Wyświetl firmę",
            "Powrót",
        };

        public static void ZarzadzajFirma(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieFirmami.Length)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzanieFirmami);

                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                Console.Clear();
                switch (wybor)
                {
                    case 1:
                        DodajFirme(centrum);
                        break;
                    case 2:
                        WyswietlFirme(centrum);
                        break;
                }
            }
        }

        private static void ZrealizujTransakcje(Centrum centrum, Firma firma)
        {
            MenuWidok.ZrealizujTransakcje(centrum, firma);
        }

        private static readonly string[] kategorie =
        {
            "Sklep",
            "Zakład usługowy",
            "Firmy transportowe",
        };

        private static readonly string firmaHeader = $"Numer{"Nazwa",15} {"Kategoria",15} {"Bank firmy",15} {"Saldo",10}";

        public static void DodajFirme(Centrum centrum)
        {
            Console.WriteLine("Podaj nazwę firmy");
            var nazwa = Console.ReadLine();

            Console.WriteLine("Wybierz kategorię firmy");
            Widok.Wyswietl(kategorie);
            var kategoria = 0;
            while (kategoria is < 1 or > 3)
                int.TryParse(Console.ReadLine(), out kategoria);
            var nowaFirma = new Firma(nazwa, kategorie[kategoria - 1], centrum);

            Console.WriteLine("Wybierz bank, w którym firma będzie posiadać konto");
            Widok.Wyswietl(centrum.Banki.Select(bank => bank.ToString()).ToList());
            var bank = 0;
            while (bank < 1 || bank > centrum.Banki.Count)
                int.TryParse(Console.ReadLine(), out bank);

            centrum.Banki[bank - 1].StworzKonto(nowaFirma);
            centrum.DodajFirme(nowaFirma);
        }

        public static void WyswietlFirmy(Centrum centrum)
        {
            Console.WriteLine(firmaHeader);
            var i = 1;
            foreach (var firma in centrum.Firmy)
                Console.WriteLine($"{i++,3}. {firma.ToString("f")}");
        }

        public static Firma WybierzFirme(Centrum centrum)
        {
            var nic = 0;
            return WybierzFirme(centrum, ref nic);
        }
        public static Firma WybierzFirme(Centrum centrum, ref int wybor)
        {
            WyswietlFirmy(centrum);
            while (wybor < 1 || wybor > centrum.Firmy.Count)
                int.TryParse(Console.ReadLine(), out wybor);

            return centrum.Firmy[wybor - 1];
        }

        public static void WyswietlFirme(Centrum centrum)
        {
            if (!MenuWidok.WeryfikujCentrum(centrum))
                return;
            
            Console.WriteLine("Podaj numer firmy, którą chcesz wyświetlić");
            var wybor = 0;
            var firma = WybierzFirme(centrum, ref wybor);
            string[] opcje =
            {
                "Zrealizuj transakcję",
                "Wyświetl transakcję",
                "Powrót",
            };
            var srodek = 0;
            while (srodek != opcje.Length)
            {
                Console.Clear();
                Console.WriteLine("Wybrana firma");
                Console.WriteLine(firmaHeader);
                Console.WriteLine($"{wybor,3}. {firma.ToString("f")}");
                Console.WriteLine("Konto firmy");
                Console.WriteLine($"   {KontoWidok.KontoHeader}");
                Widok.Wyswietl(new string[] { firma.Konta[0].ToString("b") });

                Widok.Wyswietl(opcje);
                if (!int.TryParse(Console.ReadLine(), out srodek))
                    continue;

                switch (srodek)
                {
                    case 1:
                        ZrealizujTransakcje(centrum, firma);
                        break;
                    case 2:
                        var transakcje = centrum.ZnajdzTransakcje(new Zapytanie() { Pytanie = "firma", Firma = firma });
                        foreach (var transakcja in transakcje)
                            Console.WriteLine(transakcja);
                        int.TryParse(Console.ReadLine(), out srodek);
                        break;
                }

            }
        }
    }

    public static class TransakcjaWidok
    {
        private static readonly string[] zarzadzanieTransakcjami =
        {
            "Wyświetl transakcje",
            "Znajdz transakcje",
            "Powrót",
        };

        public static void ZarzadzajTransakcjami(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieTransakcjami.Length)
            {
                if (centrum.Transakcje.Count == 0)
                {
                    Console.WriteLine("Brak transakcji");
                    Console.Read();
                    return;
                }
                Console.Clear();
                Widok.Wyswietl(zarzadzanieTransakcjami);

                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                Console.Clear();
                switch (wybor)
                {
                    case 1:
                        WyswietlTransakcje(centrum);
                        Console.Read();
                        break;
                    case 2:
                        ZnajdzTransakcje(centrum);
                        Console.Read();
                        break;
                }
            }
        }

        public static void WyswietlTransakcje(Centrum centrum)
        {
            foreach (var transakcja in centrum.Transakcje)
                Console.WriteLine(transakcja);
        }

        private static readonly string[] typy =
        {
            "Osoba",
            "Firma",
            "Bank",
            "Karta",
            "Data",
            "Kwota",
            "Szukaj",
        };

        private static readonly string[] plus =
        {
            "and",
            "or",
            "Szukaj",
        };

        public static object GetObject(Centrum centrum, int wybor)
        {
            Console.Clear();
            switch (typy[wybor - 1].ToLower())
            {
                case "osoba":
                    return OsobaWidok.WybierzOsobe(centrum);
                case "firma":
                    return FirmaWidok.WybierzFirme(centrum);
                case "bank":
                    return BankWidok.WybierzBank(centrum);
                case "karta":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Podaj numer karty");
                        if (!int.TryParse(Console.ReadLine(), out var numer)) continue;
                        try
                        {
                            return centrum.GetKarta(numer);
                        }
                        catch (Exception)
                        {
                            Console.Write("Karta o podanym numerze nie istnieje");
                            Console.Read();
                        }
                    }
                case "kwota":
                    var rob = true;
                    decimal kwota = 0;
                    while (rob)
                    {
                        Console.Clear();
                        Console.WriteLine("Podaj kwotę");
                        if (decimal.TryParse(Console.ReadLine(), out kwota))
                            rob = false;
                    }

                    return kwota;
                case "data":
                    rob = true;
                    var data = new DateTime();
                    while (rob)
                    {
                        Console.Clear();
                        Console.WriteLine("Podaj datę w formacie dd.mm.yyyy");
                        if (DateTime.TryParse(Console.ReadLine(), out data))
                            rob = false;
                    }
                    return data;
                default:
                    throw new Exception();
            }
        }

        public static Zapytanie GetZapytanie(Centrum centrum)
        {
            var pytanie = "";
            var wybor = 0;
            var done = new bool[typy.Length];
            var times = 0;
            var zapytanieObiekty = new List<object>();

            while (wybor != typy.Length)
            {
                times++;
                Console.WriteLine(pytanie);
                Widok.Wyswietl(typy);

                wybor = 0;
                while (wybor < 1 || wybor > typy.Length)
                    int.TryParse(Console.ReadLine(), out wybor);
                if (wybor == typy.Length)
                {
                    Console.Write("Nie można użyć pustego zapytania");
                    throw new Exception();
                }
                pytanie += typy[wybor - 1];
                done[wybor - 1] = true;
                zapytanieObiekty.Add(GetObject(centrum, wybor));

                if (times == typy.Length - 1)
                    return new Zapytanie(pytanie, zapytanieObiekty);

                wybor = 0;
                Console.Clear();
                Widok.Wyswietl(plus);
                while (wybor < 1 || wybor > plus.Length)
                    int.TryParse(Console.ReadLine(), out wybor);

                if (wybor == plus.Length)
                    return new Zapytanie(pytanie, zapytanieObiekty);
                pytanie += $" {plus[wybor - 1]} ";
                Console.Clear();
            }

            throw new NotImplementedException();
        }

        public static void ZnajdzTransakcje(Centrum centrum)
        {
            Console.WriteLine("Wyszukaj transakcji według:");
            var zapytanie = GetZapytanie(centrum);
            var transakcje = centrum.ZnajdzTransakcje(zapytanie);
            Console.Clear();
            Widok.Wyswietl(transakcje.Select(transakcja => transakcja.ToString()).ToList());
            Console.Read();
        }
    }
}