using System;

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
        public bool PoprosOAutoryzacje(Karta karta, decimal kwota) => Centrum.AutoryzujTransakcje(this, karta, kwota);

        public override bool Equals(object obj) => obj is Firma druga && druga.Nazwa == Nazwa;

        public override string ToString() => ToString("s");

        public override string ToString(string type)
        {
            return type switch
            {
                "f" => $"{Nazwa,25} {Kategoria,20} {konta[0].Bank.Nazwa,15} {konta[0].ToString("s"),10}",
                "s" => $"{Nazwa,10} {Kategoria,15}",
                "j" => Nazwa,
                "b" => $"{Nazwa,20} {Kategoria,20}",
                _ => throw new Exception(type)
            };
        }
        public FirmaJson makeJson() => new FirmaJson(this);
    }
}
