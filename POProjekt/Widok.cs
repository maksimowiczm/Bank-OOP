using System;
using System.Collections.Generic;

namespace POProjekt
{
    public static class Widok
    {
        public static void Wyswietl(List<string> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {lista[i]}");
            }
        }
    }
}
