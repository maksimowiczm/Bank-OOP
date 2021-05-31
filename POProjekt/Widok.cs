using System;
using System.Collections.Generic;

namespace POProjekt
{
    public abstract class Widok
    {
        public List<string> lista;
        protected int wybor;
        protected Widok(List<string> lista)
        {
            this.lista = lista;
        }

        public virtual void Wyswietl()
        {
            for (int i = 1; i <= lista.Count; i++)
            {
                Console.WriteLine($"{i}. {lista[i]}");
            }
        }
    }
}
