using Newtonsoft.Json;
using System.IO;

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

        public class DebetowaJson : Json
        {
            public readonly int KontoHash;
            public readonly int Numer;
            public readonly int OsobaHash;
            public readonly int BankHash;

            [JsonConstructor]
            public DebetowaJson(int kontoHash, int numer, int osobaHash, int bankHash, int hash) : base(hash)
            {
                KontoHash = kontoHash;
                OsobaHash = osobaHash;
                BankHash = bankHash;
                Numer = numer;
            }
            public DebetowaJson(Debetowa debetowa) : base(debetowa)
            {
                KontoHash = debetowa.Konto.GetHashCode();
                Numer = debetowa.Numer;
                OsobaHash = debetowa.Osoba.GetHashCode();
                BankHash = debetowa.Bank.GetHashCode();
            }
        }

        public DebetowaJson makeJson() => new DebetowaJson(this);
        public override void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new DebetowaJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/debetowa/{GetHashCode()}.json", json);
        }

        public static DebetowaJson Wczytaj(string dir) => JsonConvert.DeserializeObject<DebetowaJson>(File.ReadAllText(dir));

        public override string ToString() => $"{base.ToString()} {Konto.Saldo,10}";
    }
}
