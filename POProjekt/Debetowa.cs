using Newtonsoft.Json;
using System.IO;

namespace POProjekt
{
    public class Debetowa : Karta
    {
        public readonly Konto Konto;
        [JsonConstructor]
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

        internal class DebetowaJson : Json
        {
            public readonly int KontoId;
            public readonly int Numer;

            public DebetowaJson(Debetowa debetowa) : base(debetowa)
            {
                KontoId = debetowa.Konto.GetHashCode();
                Numer = debetowa.Numer;
            }
        }
        public override void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new DebetowaJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }
    }
}
