using Newtonsoft.Json;
using System.Collections.Generic;

namespace POProjekt
{
    public class Osoba : Klient
    {
        public readonly string Imie;
        public readonly string Nazwisko;
        private readonly List<Karta> karty;
        public IList<Karta> Karty => karty.AsReadOnly();

        [JsonConstructor]
        public Osoba(string imie, string nazwisko, List<Karta> karty, List<Konto> konta)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            this.karty = karty;
            this.konta = konta;
        }
        public Osoba(string imie, string nazwisko) : this(imie, nazwisko, new List<Karta>(), new List<Konto>()) { }

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
    }
}