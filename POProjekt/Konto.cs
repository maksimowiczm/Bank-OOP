using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public class Konto
    {
        public readonly Bank Bank;
        public readonly Klient Klient;
        public decimal Saldo { get; private set; }

        [JsonConstructor]
        public Konto(Bank bank, Klient klient, decimal saldo)
        {
            Klient = klient;
            Bank = bank;
            Saldo = saldo;
        }
        public Konto(Bank bank, Klient klient) : this(bank, klient, 0) { }

        public void Wplac(decimal kwota)
        {
            if (kwota <= 0) throw new KwotaException(kwota);
            Saldo += kwota;
        }
        public bool Wyplac(decimal kwota)
        {
            if (kwota <= 0) throw new WyplacException(kwota);
            if (kwota > Saldo) return false;
            Saldo -= kwota;
            return true;
        }
    }
}
