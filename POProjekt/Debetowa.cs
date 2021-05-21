using Newtonsoft.Json;

namespace POProjekt
{
    public class Debetowa : Karta
    {
        private readonly Konto konto;
        [JsonConstructor]
        public Debetowa(Bank bank, Klient klient, int numer, Konto konto) : base(bank, klient, numer)
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
