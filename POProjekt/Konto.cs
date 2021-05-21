using System;

namespace POProjekt
{
    public class Konto
    {
        public readonly Bank Bank;
        public readonly Klient Klient;
        private decimal saldo;

        public Konto(Bank bank, Klient klient) : this(bank, klient, 0) { }
        public Konto(Bank bank, Klient klient, decimal saldo)
        {
            Klient = klient;
            Bank = bank;
            this.saldo = saldo;
        }

        public void Wplac(decimal kwota)
        {
            if (kwota <= 0) throw new Exception("Ujemna kwota");
            saldo += kwota;
        }

        public bool Wyplac(decimal kwota)
        {
            if (kwota <= 0) throw new Exception("Ujemna kwota");
            if (kwota > saldo) return false;
            saldo -= kwota;
            return true;
        }
    }
}
