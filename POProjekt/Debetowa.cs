using Newtonsoft.Json;

namespace POProjekt
{
    public class Debetowa : Karta
    {
        private readonly Konto konto;
        [JsonConstructor]
        public Debetowa(Bank bank, Osoba osoba, Konto konto, int numer) : base(bank, osoba, numer)
        {
            this.konto = konto;
        }
        public Debetowa(Bank bank, Osoba osoba, Konto konto) : base(bank, osoba)
        {
            this.konto = konto;
        }
        /// <summary> Wpłaca podaną kwotę na konto tej karty. </summary>
        public override void Wplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            konto.Wplac(kwota);
        }
        /// <summary> Próbuje wypłacić podaną kwotę z konta. </summary>
        public override bool Wyplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            return konto.Wyplac(kwota);
        }
    }
}
