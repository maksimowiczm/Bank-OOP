using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POProjekt
{
    public abstract class Karta
    {
        public static int ilosc;

        public readonly int IdBanku;
        public readonly int IdKlienta;
        public readonly string Numer;
        protected decimal saldo;
        public decimal Saldo { get => saldo; }

        public Karta(int idBanku, int idKlienta)
            :this(idBanku, idKlienta, 0) { }
        public Karta(int idBanku, int idKlienta, decimal saldo)
            :this(idBanku, idKlienta, saldo, $"{ilosc++}") { }
        public Karta(int idBanku, int idKlienta, decimal saldo, string num)
        {
            if (saldo < 0)
                throw new Exception("Ujemne saldo");
            if(idBanku < 0)
                throw new Exception("Ujemny identyfikator banku");
            if(idKlienta < 0)
                throw new Exception("Ujemny identyfikator klienta");

            IdBanku = idBanku;
            IdKlienta = idKlienta;
            this.saldo = saldo;
            Numer = num;
        }

        protected bool zweryfikujKwote(decimal kwota)
        {
            if (kwota <= 0)
                throw new Exception("Niedodatnia kwota");
            return true;
        }

        public void Wplac(decimal kwota)
        {
            zweryfikujKwote(kwota);
            saldo += kwota;
        }

        public abstract bool Wyplac(decimal kwota);
    }
}
