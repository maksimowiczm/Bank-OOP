using System;

namespace POProjekt
{
    public class Firma : Klient
    {
        public readonly string Nazwa;
        public readonly string Kategoria;
        private int domyslneKonto = 0;
        public readonly Centrum Centrum;
        public int DomyslneKonto
        {
            get => domyslneKonto;
            set
            {
                if (value < konta.Count)
                    domyslneKonto = value;
            }
        }

        public Firma(string nazwa, string kategoria, Centrum centrum)
        {
            Nazwa = nazwa;
            Kategoria = kategoria;
            this.Centrum = centrum;
        }

        /// <summary> Prosi centrum o autoryzację transakcji. Jeśli się uda to wpłaca kwotę transakcji na konto firmowe. </summary>
        public bool PoprosOAutoryzacje(Karta karta, decimal kwota)
        {
            var sukces = Centrum.AutoryzujTransakcje(this, karta, kwota);
            if (!sukces) return false;

            try
            {
                konta[DomyslneKonto].Wplac(kwota);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new FirmaException(this, "Nie istnieje firmowe konto o podanym indeksie");
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is Firma druga && druga.Nazwa == Nazwa;
        }
    }
}
