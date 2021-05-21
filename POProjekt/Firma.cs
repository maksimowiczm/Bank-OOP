using System;

namespace POProjekt
{
	public class Firma: Klient
	{
		public readonly string Nazwa;
		public readonly string Kategoria;
		public readonly Centrum Centrum;

		public Firma(string nazwa, string kategoria,Centrum centrum)
		{
			Nazwa = nazwa;
			Kategoria = kategoria;
            Centrum = centrum;
        }

        public bool PoprosOAutoryzacje(Karta karta, decimal saldo)
        {
            throw new NotImplementedException();
        }
    }
}
