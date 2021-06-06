using System;

namespace POProjekt
{
    public class Konto
    {
        public readonly Bank Bank;
        public readonly Klient Klient;
        public decimal Saldo { get; private set; }

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

        public override string ToString() => ToString("s");

        public string ToString(string type)
        {
            return type switch
            {
                "f" => $"{Klient,25} {Saldo,10}",
                "B" => $"{Klient.ToString("b"),26} {Saldo,10}",
                "s" => $"{Saldo,10}",
                "b" => $"{Saldo,10} {Bank.ToString("s"),15}",
                _ => throw new Exception(type)
            };
        }
        public KontoJson makeJson() => new KontoJson(this);
    }
}
