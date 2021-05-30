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
        public readonly Centrum Centrum;

        public Firma(string nazwa, string kategoria, Centrum centrum)
        {
            Nazwa = nazwa;
            Kategoria = kategoria;
            Centrum = centrum;
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

        public override bool Equals(object? obj) => obj is Firma druga && druga.Nazwa == Nazwa;
        public override string ToString() => Nazwa;
        internal class FirmaJson : Json
        {
            public readonly string Nazwa;
            public readonly string Kategoria;
            public readonly List<int> Konta;

            public FirmaJson(Firma firma) : base(firma)
            {
                Nazwa = firma.Nazwa;
                Kategoria = firma.Kategoria;
                Konta = firma.konta.Select(konta => konta.GetHashCode()).ToList();
            }
        }
        public void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new FirmaJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }
    }
}
