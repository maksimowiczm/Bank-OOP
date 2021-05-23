using Newtonsoft.Json;

namespace POProjekt
{
    public class Debetowa : Karta
    {
        private readonly Konto konto;
        [JsonConstructor]
        public Debetowa(Bank bank, Osoba osoba, int numer, Konto konto) : base(bank, osoba, numer)
        {
            this.konto = konto;
        }
        public Debetowa(Bank bank, Osoba osoba, Konto konto) : base(bank, osoba)
        {
            this.konto = konto;
        }

        public void Wplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            konto.Wplac(kwota);
        }
        public override bool Wyplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            return konto.Wyplac(kwota);
        }
    }
}
