using POProjekt;
using System;
using System.Threading;

namespace POInterfejs
{
    public class Program
    {
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
                "Wczytaj autozapis",
                "Wyjdź",
            };
            var wybor = 0;

            while (wybor != startMenu.Length)
            {
                Console.Clear();
                Widok.WyswietlIndex(startMenu);
                int.TryParse(Console.ReadLine(), out wybor);
                switch (wybor)
                {
                    case 1:
                        MenuWidok.Start(centrum);
                        break;
                    case 2:
                        Console.WriteLine("Podaj nazwę pliku");
                        var plik = Console.ReadLine();
                        Wczytaj(centrum, plik);
                        break;
                    case 3:
                        Wczytaj(centrum, "autozapis");
                        break;
                }
            }
        }

        private static void Wczytaj(Centrum centrum, string plik)
        {
            try
            {
                centrum = Centrum.Wczytaj(plik);
                MenuWidok.Start(centrum);
            }
            catch (DeserializacjaException e)
            {
                Console.WriteLine("Krytyczny bład wczytywania");
                Console.WriteLine(e);
                Environment.Exit(1);
            }
            catch (WczytwanieZapisException)
            {
                Console.WriteLine("Bład wczytywania");
                Console.Read();
            }
        }
    }
}
