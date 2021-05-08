using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POProjekt
{
    public class Klient
    {
        private static int ilosc;
        public readonly int Id;
        public readonly string Imie;
        public readonly string Nazwisko;
        private List<Karta> karty;
        public Klient(string imie, string nazwisko, int id)
        {
            if (id < 0) throw new Exception("Id ujemne");
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Id = id;
            ilosc++;
        }
        public Klient(string imie, string nazwisko) : this(imie, nazwisko, ilosc) {}
    }
}
