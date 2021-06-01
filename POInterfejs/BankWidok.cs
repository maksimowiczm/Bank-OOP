using POProjekt;
using System;

namespace POInterfejs
{
    public static class BankWidok
    {
        private static readonly string[] zarzadzanieBankami =
        {
            "Dodaj bank",
            "Wyświetl bank",
            "Powrót",
        };

        private static readonly string bankHeader = $"Numer{"Nazwa",25} {"Ilość kart",25} {"Ilość kont",25}";

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
            if (!MenuWidok.WeryfikujCentrum(centrum))
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

            Console.Read();
        }
    }
}