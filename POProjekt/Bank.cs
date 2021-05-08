using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POProjekt
{
	public class Bank
	{
		private static int ilosc;
		private static List<Bank> banki;
		public readonly int Id;
		public readonly string Nazwa;
		private List<Karta> karty;

		static Bank GetBank(int id)
		{
			return banki.Find(b => b.Id == id); 
		}
        public Bank(string nazwa, int id, List<Karta> karty)
        {
			if (id < 0) throw new Exception("Id ujemne");
			this.Nazwa = nazwa;
			this.Id = id;
			this.karty = karty;
			ilosc++;
        }
		public Bank(string nazwa) : this(nazwa, ilosc, new List<Karta>()) { }

        
       
	}
}
