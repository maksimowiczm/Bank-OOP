using Newtonsoft.Json;
using System;
using System.IO;

namespace POProjekt
{
    public class Kredytowa : Karta
    {
        public readonly decimal Kredyt;
        private decimal saldo;
        public override decimal Saldo => saldo;

        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt, decimal saldo, int numer) : base(bank, osoba, numer)
        {
            if (kredyt <= 0)
                throw new UjemnyKredyt(kredyt);
            if (saldo < -kredyt)
                throw new KwotaException(saldo);
            Kredyt = kredyt;
            this.saldo = saldo;
        }

        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt) : this(bank, osoba, kredyt, 0) { }

        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt, decimal saldo) : base(bank, osoba)
        {
            if (kredyt <= 0)
                throw new UjemnyKredyt(kredyt);
            Kredyt = kredyt;
            this.saldo = saldo;
        }

        public override void Wplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            saldo += kwota;
        }

        public override bool Wyplac(decimal kwota)
        {
            if (!ZweryfikujKwote(kwota))
                throw new KwotaException(kwota);
            if (Saldo - kwota < -Kredyt) return false;

            saldo -= kwota;
            return true;
        }
        
        public KredytowaJson makeJson() => new KredytowaJson(this);

        public override string ToString() => $"{base.ToString()} {Kredyt,10}";
    }
}
