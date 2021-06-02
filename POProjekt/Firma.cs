using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Firma : Klient
    {
        public readonly string Nazwa;
        public readonly string Kategoria;
        private Centrum centrum;

        /// <summary> Pozwala zmienić centrum tylko podczas wczytywania z pliku. </summary>
        public Centrum Centrum
        {
            get => centrum;
            set
            {
                if (Centrum.wczytywanie)
                    centrum = value;
            }
        }

        public Firma(string nazwa, string kategoria, Centrum centrum)
        {
            Nazwa = nazwa;
            Kategoria = kategoria;
            this.centrum = centrum;
        }

        /// <summary> Prosi centrum o autoryzację transakcji. Jeśli się uda to wpłaca kwotę transakcji na konto firmowe. </summary>
        public bool PoprosOAutoryzacje(Karta karta, decimal kwota)
        {
            var sukces = Centrum.AutoryzujTransakcje(this, karta, kwota);
            if (!sukces) return false;

            try
            {
                konta[0].Wplac(kwota);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FirmaException(this, "Firma nie posiada konta");
            }

            return true;
        }

        public override bool Equals(object obj) => obj is Firma druga && druga.Nazwa == Nazwa;

        public override string ToString() => ToString("s");

        public override string ToString(string type)
        {
            return type switch
            {
                "f" => $"{Nazwa,25} {Kategoria,15} {konta[0].Bank.Nazwa,15} {konta[0].ToString("s"),10}",
                "s" => $"{Nazwa,10} {Kategoria,15}",
                "b" => $"{Nazwa,25} {Kategoria,15}",
                _ => throw new Exception(type)
            };
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
                Konta = firma.konta.Select(konta => konta.GetHashCode()).ToList();
            }
        }

        public FirmaJson makeJson() => new FirmaJson(this);
        public void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new FirmaJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }

        public static FirmaJson Wczytaj(string dir) => JsonConvert.DeserializeObject<FirmaJson>(File.ReadAllText(dir));
    }
}
