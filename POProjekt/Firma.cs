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
            if (sukces)
                konta[0].Wplac(kwota);
            return sukces;
        }
    }
}
