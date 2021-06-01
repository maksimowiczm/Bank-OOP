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
                "Wyjdź",
            };
            var wybor = 0;

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
