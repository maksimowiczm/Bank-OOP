using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POProjekt
{
    public class Klient
    {
        private static int ilosc;
        private static List<Klient> klienci = new List<Klient>();
        public static Klient GetKlient(int id) => klienci.Find(k => k.Id == id);

        public readonly int Id;
        public readonly string Imie;
        public readonly string Nazwisko;
        private List<Karta> karty;
        public List<Karta> Karty { get => karty; }

        [JsonConstructor]
        public Klient(string imie, string nazwisko, int id, List<Karta>karty)
        {
            if (id < 0) throw new Exception("Id ujemne");
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Id = id;
            ilosc++;
            this.karty = karty;
            klienci.Add(this);
        }
        public Klient(string imie, string nazwisko) : this(imie, nazwisko, ilosc, new List<Karta>()) { }

        public bool DodajKarteDebetowa(Bank bank) => DodajKarteDebetowa(bank, 0);
        public bool DodajKarteDebetowa(Bank bank, decimal saldo)
        {
            if (saldo >= 0)
                karty.Add(bank.StworzKarteDebetowa(this, saldo));
            return false;
        }
        public bool DodajKarteKredytowa(Bank bank, decimal maksymalnyKredyt, decimal saldo)
        {
            if (maksymalnyKredyt < 0) return false;
            if (saldo < -maksymalnyKredyt) return false;
            karty.Add(bank.StworzKarteKredytowa(this, maksymalnyKredyt, saldo));
            return true;
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
