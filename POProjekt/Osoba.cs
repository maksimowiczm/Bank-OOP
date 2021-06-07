using System;
using System.Collections.Generic;

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

        /// <summary> Jeśli podana karta należy do tej osoby dodaje ją do list kart. </summary>
        public bool DodajKarte(Karta karta)
        {
            if (Centrum.wczytywanie)
            {
                karty.Add(karta);
                return true;
            }
            if (!karta.Osoba.Equals(this) || karty.Contains(karta)) throw new KartaNieIstnieje(karta, this);

            karty.Add(karta);
            return true;

        }

        public override bool Equals(object obj) => obj is Osoba druga && druga.Imie == Imie && druga.Nazwisko == Nazwisko;

        public override string ToString() => ToString("s");

        public override string ToString(string type)
        {
            return type switch
            {
                "f" => $"{Imie,25}{Nazwisko,25}{karty.Count,25}{konta.Count,25}",
                "s" => $"{Imie,10} {Nazwisko,15}",
                "j" => $"{Imie} {Nazwisko}",
                "b" => $"{Imie,20} {Nazwisko,20}",
                _ => throw new Exception(type)
            };
        }

        public OsobaJson makeJson() => new OsobaJson(this);
    }
}