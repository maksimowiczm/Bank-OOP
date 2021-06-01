using POProjekt;
using System;
using System.Threading;

namespace POInterfejs
{
    public class Program
    {
        private static void przyklad(Centrum centrum)
        {
            centrum.DodajBank(new Bank("PKO"));
            centrum.DodajBank(new Bank("Pekao"));
            centrum.DodajFirme(new Firma("Rito gejms", "Sklep", centrum));
            centrum.DodajFirme(new Firma("Valve", "Sklep", centrum));
            centrum.DodajOsobe(new Osoba("Jan", "Kowalski"));
            centrum.DodajOsobe(new Osoba("Janusz", "Kowalski"));

            var osoby = centrum.Osoby;
            var firmy = centrum.Firmy;
            var banki = centrum.Banki;

            foreach (var firma in firmy)
                banki[0].StworzKonto(firma);

            banki[0].StworzKarteDebetowa(osoby[0], banki[0].StworzKonto(osoby[0]));
            banki[1].StworzKarteKredytowa(osoby[0], 1000);
            osoby[0].Konta[0].Wplac(1000);

            banki[1].StworzKarteDebetowa(osoby[1], banki[1].StworzKonto(osoby[1]));
            banki[1].StworzKarteDebetowa(osoby[1], banki[1].StworzKonto(osoby[1]));
            osoby[1].Konta[0].Wplac(10000);
            osoby[1].Konta[1].Wplac(100000);

            firmy[0].PoprosOAutoryzacje(osoby[0].Karty[0], 100);
            firmy[1].PoprosOAutoryzacje(osoby[0].Karty[0], 500);
            firmy[0].PoprosOAutoryzacje(osoby[0].Karty[1], 500);
            firmy[0].PoprosOAutoryzacje(osoby[0].Karty[1], 500);

            firmy[1].PoprosOAutoryzacje(osoby[1].Karty[0], 5000);
        }
        private const int sleep = 0;
        private const string start = "Witaj w centrum obsługi kart płatniczych";

        public static void Main()
        {
            Console.WriteLine(start);
            Thread.Sleep(sleep);
            var centrum = new Centrum();
            var startMenu = new[]
            {
                "Stwórz nowe centurm",
                "Wczytaj istniejące centrum z pliku",
                "Wyjdź",
            };
            var wybor = 0;
            przyklad(centrum);

            while (wybor != startMenu.Length)
            {
                Console.Clear();
                Widok.Wyswietl(startMenu);
                int.TryParse(Console.ReadLine(), out wybor);
                switch (wybor)
                {
                    case 1:
                        MenuWidok.Start(centrum);
                        break;
                    case 2:
                        Console.WriteLine("Podaj nazwę pliku");
                        var plik = Console.ReadLine();
                        try
                        {
                            centrum = Centrum.Wczytaj(plik);
                            MenuWidok.Start(centrum);
                        }
                        catch (WczytwanieZapisException)
                        {
                            Console.WriteLine("Bład wczytywania");
                            Console.Read();
                        }
                        break;
                }
            }
        }
    }
}
