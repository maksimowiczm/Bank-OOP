using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Osoba : Klient
    {
        public readonly string Imie;
        public readonly string Nazwisko;
        private readonly List<Karta> karty = new();
        public IList<Karta> Karty => karty.AsReadOnly();

        public Osoba(string imie, string nazwisko)
        {
            Imie = imie;
            Nazwisko = nazwisko;
        }

        private bool mojaKarta(Karta karta) => karty.Contains(karta);
        /// <summary> Jeśli podana karta należy do tej osoby dodaje ją do list kart. </summary>
        public bool DodajKarte(Karta karta)
        {
            if (karta.Osoba != this || karty.Contains(karta)) throw new KartaNieIstnieje(karta, this);

            karty.Add(karta);
            return true;

        }
        public bool UsunKarte(Karta karta)
        {
            if (!mojaKarta(karta)) throw new KartaNieIstnieje(karta, this);

            karty.Remove(karta);
            return true;
        }

        public override bool Equals(object obj) => obj is Osoba druga && druga.Imie == Imie && druga.Nazwisko == Nazwisko;
        public override string ToString() => $"{Imie} {Nazwisko}";

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
                Karty = osoba.karty.Select(karta => karta.GetHashCode()).ToList();
                Konta = osoba.konta.Select(konta => konta.GetHashCode()).ToList();
            }
        }
        public void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new OsobaJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }
        public static OsobaJson Wczytaj(string dir) => JsonConvert.DeserializeObject<OsobaJson>(File.ReadAllText(dir));
    }
}