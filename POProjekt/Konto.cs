using System;

namespace POProjekt
{
    public class Konto
    {
        public readonly int idBanku;
        private decimal saldo;
        public decimal Saldo { get => saldo; }

        public Konto(Bank bank, decimal saldo)
        {
            this.idBanku = bank.Id;
            this.saldo = saldo;
        }
        public Konto(Bank bank) : this(bank, 0) { }

        public void Wplac(decimal kwota)
        {
            if (kwota <= 0)
                throw new Exception("Niedodatnia kwota");
            saldo += kwota;
        }
    }
}
