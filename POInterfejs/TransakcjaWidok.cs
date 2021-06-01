using POProjekt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POInterfejs
{
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