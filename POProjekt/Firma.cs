namespace POProjekt
{
    public class Firma
    {
        private static int ilosc;
        public readonly string Nazwa;
        public readonly string Kategoria;
        public readonly int Id;
        public Konto Konto { get; }

        public Firma(string nazwa, string kategoria, Konto konto, int id)
        {
            Id = id;
            ilosc++;
            Nazwa = nazwa;
            Kategoria = kategoria;
            this.Konto = konto;
        }
        public Firma(string nazwa, string kategoria, Bank bank) : this(nazwa, kategoria, bank.StworzKonto(ilosc), ilosc) { }
        public Firma(string nazwa, string kategoria, Bank bank, decimal saldo) : this(nazwa, kategoria, bank.StworzKonto(ilosc, saldo), ilosc) { }
        public bool PoprosOAutoryzacje(Karta karta, decimal kwota)
        {
            if (kwota > 0)
            {
                var sukces = Centrum.AutoryzujTransakcje(this, karta, kwota);
                if (sukces)
                    Konto.Wplac(kwota);
                return sukces;
            }
            else return false;
        }
    }
}
