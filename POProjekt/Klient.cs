using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace POProjekt
{
	public class Klient
	{
		private static int ilosc;
		public readonly int Id;
		public readonly string Imie;
		public readonly string Nazwisko;
		private List<Karta> karty;
		[JsonConstructor]
		public Klient(string imie, string nazwisko, int id)
		{
			if (id < 0) throw new Exception("Id ujemne");
			this.Imie = imie;
			this.Nazwisko = nazwisko;
			this.Id = id;
			ilosc++;
		}
		public Klient(string imie, string nazwisko) : this(imie, nazwisko, ilosc) {}

		public bool DodajKarteDebetowa(Bank bank) => DodajKarteDebetowa(bank, 0);

		public bool DodajKarteDebetowa(Bank bank, decimal saldo)
		{
			if(saldo>=0)
			{
				karty.Add(bank.StworzKarteDebetowa(this, saldo));
			}
			return false;
		}
		public bool DodajKarteKredytowa(Bank bank, decimal maksymalnyKredyt, decimal saldo)
        {
			if (maksymalnyKredyt < 0) return false;
			if (saldo < -maksymalnyKredyt) return false;
			karty.Add(bank.StworzKarteKredytowa(this,maksymalnyKredyt,saldo));
			return true;
        }
		public bool DodajKarteKredytowa(Bank bank, decimal maksymalnyKredyt) => DodajKarteKredytowa(bank, maksymalnyKredyt, 0);
		private int mojaKarta(Karta karta) => karty.IndexOf(karta);
		public bool UsunKarte(Karta karta)
		{
			if (mojaKarta(karta) > -1)
			{
				karty.Remove(karta);
				return true;
			}
			return false;
		}
	}
}
