using POProjekt;
using System;
using System.Linq;

namespace POInterfejs
{
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
}