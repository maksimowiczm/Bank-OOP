namespace POProjekt
{
    public class Debetowa : Karta
    {
        public readonly Konto Konto;
        public override decimal Saldo => Konto.Saldo;

        public Debetowa(Bank bank, Osoba osoba, Konto konto, int numer) : base(bank, osoba, numer)
        {
            Konto = konto;
        }

        public Debetowa(Bank bank, Osoba osoba, Konto konto) : base(bank, osoba)
        {
            Konto = konto;
        }

        /// <summary> Wpłaca podaną kwotę na konto tej karty. </summary>
        public override void Wplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            Konto.Wplac(kwota);
        }

        /// <summary> Próbuje wypłacić podaną kwotę z konta. </summary>
        public override bool Wyplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            return Konto.Wyplac(kwota);
        }

        public DebetowaJson makeJson() => new DebetowaJson(this);
    }
}
