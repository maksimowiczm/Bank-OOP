using POProjekt;
using System;
using System.Linq;

namespace POInterfejs
{
    public static class MenuWidok
    {
        private static readonly string[] menu =
        {
            "Zarządzanie bankami",
            "Zarządzanie osobami",
            "Zarządzanie firmami",
            "Zarządzanie transakcjami",
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

                try { Zapisz(centrum, "autozapis"); }
                catch (Exception e) { Console.WriteLine(e); }

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
            var stworzone = new bool[3];
            if (centrum.Banki.Count == 0)
                stworzone[0] = true;
            if (centrum.Osoby.Count == 0)
                stworzone[1] = true;
            if (centrum.Firmy.Count == 0)
                stworzone[2] = true;

            if (!stworzone.Any(b => b)) return true;

            Console.WriteLine($"Najpierw stwórz: {(stworzone[0] ? "bank/" : "")}{(stworzone[1] ? "osobę/" : "")}{(stworzone[2] ? "firmę" : "")}");
            Console.Read();
            return false;
        }
        public static void ZrealizujTransakcje(Centrum centrum, Firma firma)
        {
            Console.Clear();
            Console.WriteLine("Wybierz osobę do transakcji");
            var osoba = OsobaWidok.WybierzOsobe(centrum);

            if (osoba.Karty.Count == 0)
            {
                Console.WriteLine("Osoba nie posiada kart");
                Console.Read();
                return;
            }

            Console.Clear();
            Console.WriteLine("Podaj Kwotę transakcji");
            decimal kwota;
            while (!decimal.TryParse(Console.ReadLine(), out kwota)) { }

            Console.Clear();
            Console.WriteLine("Wybierz kartę do zapłaty");

            var karta = KartaWidok.WybierzKarte(osoba);

            Console.Clear();
            Console.WriteLine("Realizuj transakcję");
            Widok.WyswietlIndex(new string[] { "tak", "nie" });
            var wybor = 0;
            while (wybor is < 1 or > 2)
                int.TryParse(Console.ReadLine(), out wybor);
            if (wybor == 2)
                return;

            var sukces = firma.PoprosOAutoryzacje(karta, kwota);
            Console.WriteLine($"Transakcja{(sukces ? "" : " nie")} udana.");
            Console.WriteLine(centrum.Transakcje.Last().ToString());
            Console.Read();
        }
        public static void ZrealizujTransakcje(Centrum centrum)
        {
            if (!WeryfikujCentrum(centrum))
                return;
            Console.Clear();
            Console.WriteLine("Wybierz firmę do transakcji");
            var firma = FirmaWidok.WybierzFirme(centrum);
            ZrealizujTransakcje(centrum, firma);
        }

        public static void Wyswietl() => Widok.WyswietlIndex(menu);

        public static void Zapisz(Centrum centrum, string nazwa) => centrum.Zapisz(nazwa);

        public static void Zapisz(Centrum centrum)
        {
            Console.Clear();
            Console.WriteLine("Podaj nazwę zapisu");
            var nazwa = Console.ReadLine();
            Zapisz(centrum, nazwa);
        }
    }
}