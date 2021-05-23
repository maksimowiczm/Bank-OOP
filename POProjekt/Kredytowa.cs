using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public class Kredytowa : Karta
    {
        public readonly decimal MaksymalnyKredyt;
        public decimal Saldo { get; private set; }

        [JsonConstructor]
        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt, decimal saldo, int numer)
            : base(bank, osoba, numer)
        {
            if (kredyt <= 0)
                throw new Exception("Niedodatni kredyt");
            if (saldo < -kredyt)
                throw new Exception("Za niskie saldo");
            MaksymalnyKredyt = kredyt;
            this.Saldo = saldo;
        }
        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt) : this(bank, osoba, kredyt, 0) { }
        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt, decimal saldo) : base(bank, osoba)
        {
            if (kredyt <= 0)
                throw new Exception("Niedodatni kredyt");
            MaksymalnyKredyt = kredyt;
            this.Saldo = saldo;
        }

        public override bool Wyplac(decimal kwota)
        {
            if (!ZweryfikujKwote(kwota))
                return false;
            if (Saldo - kwota < -MaksymalnyKredyt) return false;

            Saldo -= kwota;
            return true;
        }
    }
}
