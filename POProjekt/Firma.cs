namespace POProjekt
{
    public class Firma
    {
        public readonly string Nazwa;
        public readonly string Kategoria;
        public Konto Konto { get; }

        public Firma(string nazwa, string kategoria, Konto konto)
        {
            Nazwa = nazwa;
            Kategoria = kategoria;
            this.Konto = konto;
        }
        public Firma(string nazwa, string kategoria, Bank bank) : this(nazwa, kategoria, bank.StworzKonto()) { }
        public Firma(string nazwa, string kategoria, Bank bank, decimal saldo) : this(nazwa, kategoria, bank.StworzKonto(saldo)) { }
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
