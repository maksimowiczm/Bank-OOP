using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Zapis
    {
        public List<KredytowaJson> Kredytowe { get; init; }
        public List<DebetowaJson> Debetowe { get; init; }
        public List<KontoJson> Konta { get; init; }
        public List<BankJson> Banki { get; init; }
        public List<OsobaJson> Osoby { get; init; }
        public List<FirmaJson> Firmy { get; init; }
        public List<Transakcja> Transakcje { get; init; }

        public void Zapisz(string plik)
        {
            File.WriteAllText(plik, JsonConvert.SerializeObject(this, Json.JsonSerializerSettings));
        }
    }

    /// <summary> Używana do zapisu/odczytu z dysku. </summary>
    public class Json
    {
        public static JsonSerializerSettings JsonSerializerSettings = new()
        {
            Formatting = Formatting.None,
        };

        public readonly int Hash;

        public Json(int hash)
        {
            Hash = hash;
        }

        public Json(object obj)
        {
            Hash = obj.GetHashCode();
        }
    }

    public class BankJson : Json
    {
        public readonly string Nazwa;
        public readonly List<int> Karty;
        public readonly List<int> Konta;
        [JsonConstructor]
        public BankJson(string nazwa, List<int> karty, List<int> konta, int hash) : base(hash)
        {
            Nazwa = nazwa;
            Karty = karty;
            Konta = konta;
        }
        public BankJson(Bank bank) : base(bank)
        {
            Nazwa = bank.Nazwa;
            Karty = bank.Karty.Select(karta => karta.GetHashCode()).ToList();
            Konta = bank.Konta.Select(konto => konto.GetHashCode()).ToList();
        }
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

    public class KredytowaJson : Json
    {
        public readonly decimal Kredyt;
        public readonly decimal Saldo;
        public readonly int Numer;
        public readonly int BankHash;
        public readonly int OsobaHash;

        [JsonConstructor]
        public KredytowaJson(decimal kredyt, decimal saldo, int numer, int hash, int bankHash, int osobaHash) : base(hash)
        {
            Kredyt = kredyt;
            Saldo = saldo;
            Numer = numer;
            BankHash = bankHash;
            OsobaHash = osobaHash;
        }
        public KredytowaJson(Kredytowa kredytowa) : base(kredytowa)
        {
            Kredyt = kredytowa.Kredyt;
            Saldo = kredytowa.Saldo;
            Numer = kredytowa.Numer;
            BankHash = kredytowa.Bank.GetHashCode();
            OsobaHash = kredytowa.Osoba.GetHashCode();
        }
    }

    public class OsobaJson : Json
    {
        public readonly string Imie;
        public readonly string Nazwisko;
        public readonly List<int> Karty;
        public readonly List<int> Konta;

        [JsonConstructor]
        public OsobaJson(string imie, string nazwisko, List<int> karty, List<int> konta, int hash) : base(hash)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Karty = karty;
            Konta = konta;
        }
        public OsobaJson(Osoba osoba) : base(osoba)
        {
            Imie = osoba.Imie;
            Nazwisko = osoba.Nazwisko;
            Karty = osoba.Karty.Select(karta => karta.GetHashCode()).ToList();
            Konta = osoba.Konta.Select(konta => konta.GetHashCode()).ToList();
        }
    }

    public class FirmaJson : Json
    {
        public readonly string Nazwa;
        public readonly string Kategoria;
        public readonly List<int> Konta;
        [JsonConstructor]
        public FirmaJson(string nazwa, string kategoria, List<int> konta, int hash) : base(hash)
        {
            Nazwa = nazwa;
            Kategoria = kategoria;
            Konta = konta;
        }
        public FirmaJson(Firma firma) : base(firma)
        {
            Nazwa = firma.Nazwa;
            Kategoria = firma.Kategoria;
            Konta = firma.Konta.Select(konta => konta.GetHashCode()).ToList();
        }
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
}
