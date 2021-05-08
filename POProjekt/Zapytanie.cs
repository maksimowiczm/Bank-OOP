using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POProjekt
{
	public class Zapytanie
    {
        public readonly DateTime Data;
        public readonly Bank Bank;
        public readonly Firma Firma;
        public readonly Klient Klient;
        public readonly string NrKarty;
        public readonly decimal Kwota;

        public Zapytanie(DateTime data, Bank bank, Firma firma, Klient klient, string nrKarty, decimal kwota)
        {
            Data = data;
            Bank = bank;
            Firma = firma;
            Klient = klient;
            NrKarty = nrKarty;
            Kwota = kwota;
        }
    }
}
