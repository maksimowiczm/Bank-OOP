namespace POProjekt
{
    public class Firma
    {
        public readonly string Nazwa;
        public Konto Konto { get; }

        public Firma(string nazwa, Konto konto)
        {
            Nazwa = nazwa;
            this.Konto = konto;
        }
        public Firma(string nazwa, Bank bank) : this(nazwa, bank.StworzKonto()) { }
        public Firma(string nazwa, Bank bank, decimal saldo) : this(nazwa, bank.StworzKonto(saldo)) { }
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
