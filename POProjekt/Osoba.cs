using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Osoba : Klient
    {
        public readonly string Imie;
        public readonly string Nazwisko;
        private List<Karta> karty;
        public List<Karta> Karty => new List<Karta>(karty);

        [JsonConstructor]
        public Osoba(string imie, string nazwisko, List<Karta> karty, List<Konto>konta)
        {
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.karty = karty;
            this.konta = konta;
        }
        public Osoba(string imie, string nazwisko) : this(imie, nazwisko, new List<Karta>(), new List<Konto>()) { }

        public bool DodajKarteDebetowa()
        {
            throw new NotImplementedException();
        }
        public bool DodajKarteKredytowa(Bank bank, decimal maksymalnyKredyt, decimal saldo)
        {
            if (maksymalnyKredyt < 0) return false;
            if (saldo < -maksymalnyKredyt) return false;
            throw new NotImplementedException();
        }
        public bool DodajKarteKredytowa(Bank bank, decimal maksymalnyKredyt) => DodajKarteKredytowa(bank, maksymalnyKredyt, 0);
        private int mojaKarta(Karta karta) => karty.IndexOf(karta);
        public bool UsunKarte(Karta karta)
        {
            if (mojaKarta(karta) < 0) return false;
            karty.Remove(karta);
            return true;
        }
    }
}