using System;

namespace POProjekt
{
    public class Konto
    {
        public readonly int idBanku;
        public readonly int idFirmy;
        private decimal saldo;
        public decimal Saldo { get => saldo; }

        public Konto(int idBanku, int idFirmy, decimal saldo)
        {
            this.idBanku = idBanku;
            this.idFirmy = idFirmy;
            this.saldo = saldo;
        }
        public Konto(int idBanku, int idFirmy) : this(idBanku, idFirmy, 0) { }

        public void Wplac(decimal kwota)
        {
            if (kwota <= 0)
                throw new Exception("Niedodatnia kwota");
            saldo += kwota;
        }
    }
}
