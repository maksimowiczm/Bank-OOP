using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POProjekt
{
    public static class Widok
    {
        public static void Wyswietl(List<string> lista)
        {
            for (var i = 0; i < lista.Count; i++)
                Console.WriteLine($"{i + 1}. {lista[i]}");
        }
    }

    public static class MenuWidok
    {
        public static readonly List<string> menu = new()
        {
            "Zarządzanie bankami",
            "Zarządzanie osobami",
            "Zarządzanie firmami",
            "Wyświetl trasakcje",
            "Zapisz",
            "Wyjdz",
        };

        public static void Wyswietl() => Widok.Wyswietl(menu);

        public static void Zapisz(Centrum centrum, string nazwa) => centrum.Zapisz(nazwa);

        public static void Zapisz(Centrum centrum)
        {
            Console.Clear();
            Console.WriteLine("Podaj nazwę zapisu:");
            var nazwa = Console.ReadLine();
            Zapisz(centrum, nazwa);
        }

        public static void WyswietlTransakcje(Centrum centrum)
        {
            Console.Clear();
            var transakcje = centrum.Transakcje.Select(transakcja => transakcja.Json()).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(transakcje, Formatting.Indented));

            Console.Read();
        }


        private static readonly List<string> zarzadzanieBankami = new()
        {
            "Dodaj bank",
            "Wyświetl banki",
            "Powrót",
        };
        public static void ZarzadzajBank(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieBankami.Count)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzanieBankami);

                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                switch (wybor)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Podaj nazwę banku");
                        centrum.DodajBank(new Bank(Console.ReadLine()));
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine($"{"Nazwa",25} {"Ilość kart",25} {"Ilość kont",25}");
                        foreach (var bank in centrum.Banki)
                            Console.WriteLine(bank);
                        Console.Read();
                        break;
                }
            }
        }

        private static readonly List<string> zarzadzanieFirmami = new()
        {
            "Dodaj frimę",
            "Wyświetl firmy",
            "Powrót",
        };
        private static readonly List<string> kategorie = new()
        {
            "Sklep",
            "Zakład usługowy",
            "Firmy transportowe",
        };
        public static void ZarzadzajFirma(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieFirmami.Count)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzanieFirmami);

                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                switch (wybor)
                {
                    case 1:
                        Console.Clear();
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
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine($"{"Nazwa",25}{"Kategoria",25}{"Bank firmy",25}{"Saldo konta",25}");
                        foreach (var firma in centrum.Firmy)
                            Console.WriteLine(firma);
                        Console.Read();
                        break;
                }
            }
        }

        private static readonly List<string> zarzadzanieOsobami = new()
        {
            "Dodaj osobę",
            "Wyświetl osoby",
            //"Edytuj osobę",
            "Powrót",
        };
        public static void ZarzadzajOsoba(Centrum centrum)
        {
            var wybor = 0;
            while (wybor != zarzadzanieOsobami.Count)
            {
                Console.Clear();
                Widok.Wyswietl(zarzadzanieOsobami);

                if (!int.TryParse(Console.ReadLine(), out wybor)) continue;

                switch (wybor)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Podaj imię");
                        var imie = Console.ReadLine();
                        Console.WriteLine("Podaj nazwisko");
                        var nazwisko = Console.ReadLine();
                        centrum.DodajOsobe(new Osoba(imie, nazwisko));
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine($"{"Imie",25}{"Nazwisko",25}{"Ilość kart",25}{"Ilość kont",25}");
                        foreach (var osoba in centrum.Osoby)
                            Console.WriteLine(osoba);
                        Console.Read();
                        break;
                }
            }
        }
    }
    public static class BankWidok
    {
        private static readonly List<string> bankWidok = new()
        {
            "Wyświetl karty",
            "Wyświetl konta",
        };
        public static void Wyswietl() => Widok.Wyswietl(bankWidok);
    }
}