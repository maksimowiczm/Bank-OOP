using Newtonsoft.Json;
using System.IO;

namespace POProjekt
{
    public class Konto
    {
        public readonly Bank Bank;
        public readonly Klient Klient;
        public decimal Saldo { get; private set; }

        public Konto(Bank bank, Klient klient, decimal saldo)
        {
            Klient = klient;
            Bank = bank;
            Saldo = saldo;
        }
        public Konto(Bank bank, Klient klient) : this(bank, klient, 0) { }

        public void Wplac(decimal kwota)
        {
            if (kwota <= 0) throw new KwotaException(kwota);
            Saldo += kwota;
        }
        public bool Wyplac(decimal kwota)
        {
            if (kwota <= 0) throw new WyplacException(kwota);
            if (kwota > Saldo) return false;
            Saldo -= kwota;
            return true;
        }

        public class KontoJson : Json
        {
            public readonly decimal Saldo;
            public readonly int KlientHash;
            public readonly int BankHash;

            [JsonConstructor]
            public KontoJson(int hash, decimal saldo, int klientHash, int bankHash) : base(hash)
            {
                Saldo = saldo;
                KlientHash = klientHash;
                BankHash = bankHash;
            }
            public KontoJson(Konto obj) : base(obj)
            {
                KlientHash = obj.Klient.GetHashCode();
                BankHash = obj.Bank.GetHashCode();
                Saldo = obj.Saldo;
            }
        }
        public void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new KontoJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }
        public static KontoJson Wczytaj(string dir) => JsonConvert.DeserializeObject<KontoJson>(File.ReadAllText(dir));
    }
}
