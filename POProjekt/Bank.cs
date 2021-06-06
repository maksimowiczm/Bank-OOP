using System;
using System.Collections.Generic;

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

        private bool mojaKarta(Karta karta) => karty.Contains(karta);

        /// <summary> Używane tylko do wczytywania z pliku. Dodaje podaną kartę do listy kart w banku. </summary>
        public void DodajKarte(Karta karta)
        {
            if (Centrum.wczytywanie)
                karty.Add(karta);
        }

        public bool RealizujTransakcje(Karta karta, decimal kwota) => mojaKarta(karta) && karta.Wyplac(kwota);

        public override bool Equals(object obj) => obj is Bank drugi && drugi.Nazwa == Nazwa;

        public override string ToString() => ToString("f");

        public string ToString(string type)
        {
            return type switch
            {
                "f" => $"{Nazwa,25} {karty.Count,25} {konta.Count,25}",
                "s" => $"{Nazwa,10}",
                "j" => Nazwa,
                _ => throw new Exception(type)
            };
        }

        public BankJson makeJson() => new BankJson(this);
    }
}