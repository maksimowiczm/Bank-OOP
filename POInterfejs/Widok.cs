using System;
using System.Collections.Generic;

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
}