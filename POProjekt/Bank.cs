using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Bank
    {
        public readonly string Nazwa;
        private readonly List<Karta> karty = new();
        private readonly List<Konto> konta = new();
        public IList<Karta> Karty => karty.AsReadOnly();
        public IList<Konto> Konta => konta.AsReadOnly();

        public Bank(string nazwa)
        {
            Nazwa = nazwa;
        }

        /// <summary> Używane tylko do wczytywania z pliku. Dodaje podane konto do listy kont w banku. </summary>
        public void DodajKonto(Konto konto)
        {
            if (Centrum.wczytywanie)
                konta.Add(konto);
        }
        private bool mojeKonto(Konto konto) => konta.Contains(konto);

        /// <summary> Tworzy nowe konto dla podanego klienta, dodaje je do swojej listy kont i list kont klienta. </summary>
        /// <returns> Stworzone konto. </returns>
        public Konto StworzKonto(Klient klient)
        {
            var konto = new Konto(this, klient);
            konta.Add(konto);
            klient.DodajKonto(konto);
            return konto;
        }

        /// <summary> Próbuje usunąć podane konto. </summary>
        public bool UsunKonto(Konto konto)
        {
            if (!konta.Contains(konto) || !konto.Klient.UsunKonto(konto))
                throw new KontoNieIstnieje(konto, this);

            konta.Remove(konto);
            return true;
        }

        /// <summary>
        /// Tworzy nową kartę debetową jeśli podane konto należy do tego banku i osoby. Dodaje je do listy kart w banku i listy kart klienta.
        /// </summary>
        /// <returns>Karta Debetowa</returns>
        public Debetowa StworzKarteDebetowa(Osoba osoba, Konto konto)
        {
            if (!mojeKonto(konto))
                throw new KontoNieIstnieje(konto, this);
            if (!osoba.MojeKonto(konto))
                throw new KontoNieIstnieje(konto, osoba);

            var karta = new Debetowa(this, osoba, konto);
            karty.Add(karta);
            osoba.DodajKarte(karta);
            return karta;
        }

        /// <summary> Tworzy nową kartę kredytową. Dodaje je do listy kart w banku i listy kart klienta. </summary>
        /// <param name="kredyt">Maksymalny kredyt jaki moża wziąć karta, czyli na ile maksymalnie na minus może wyjść saldo</param>
        /// <returns>Karta Kredytowa</returns>
        public Kredytowa StworzKarteKredytowa(Osoba osoba, decimal kredyt)
        {
            var karta = new Kredytowa(this, osoba, kredyt);
            karty.Add(karta);
            osoba.DodajKarte(karta);
            return karta;
        }

        /// <summary> Używane tylko do wczytywania z pliku. Dodaje podaną kartę do listy kart w banku. </summary>
        public void DodajKarte(Karta karta)
        {
            if (Centrum.wczytywanie)
                karty.Add(karta);
        }

        private bool mojaKarta(Karta karta) => karty.Contains(karta);

        /// <summary> Próbuje usunąć podaną kartę. </summary>
        public bool UsunKarte(Karta karta)
        {
            if (!mojaKarta(karta)) throw new KartaNieIstnieje(karta, this);
            karty.Remove(karta);
            karta.Osoba.UsunKarte(karta);
            return true;
        }

        public bool RealizujTransakcje(Karta karta, decimal kwota) => mojaKarta(karta) && karta.Wyplac(kwota);

        public override bool Equals(object obj) => obj is Bank drugi && drugi.Nazwa == Nazwa;
        public override string ToString() => Nazwa;

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
                Karty = bank.karty.Select(karta => karta.GetHashCode()).ToList();
                Konta = bank.konta.Select(konto => konto.GetHashCode()).ToList();
            }
        }
        public void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new BankJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }
        public static BankJson Wczytaj(string dir) => JsonConvert.DeserializeObject<BankJson>(File.ReadAllText(dir));
    }
}
