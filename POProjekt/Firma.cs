using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POProjekt
{
	public class Firma
	{
		public readonly string Nazwa;

		public Firma(string nazwa)
		{
			this.Nazwa = nazwa;
		}
		public bool PoprosOAutoryzacje(Karta karta, decimal kwota)
		{
			if (kwota > 0)
				return Centrum.AutoryzujTransakcje(this, karta, kwota);
			else return false;
		}
	}
}
