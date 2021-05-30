using Newtonsoft.Json;
using System;
using System.IO;

namespace POProjekt
{
    public class Kredytowa : Karta
    {
        public readonly decimal Kredyt;
        public decimal Saldo { get; private set; }

        [JsonConstructor]
        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt, decimal saldo, int numer)
            : base(bank, osoba, numer)
        {
            if (kredyt <= 0)
                throw new Exception("Niedodatni kredyt");
            if (saldo < -kredyt)
                throw new Exception("Za niskie saldo");
            Kredyt = kredyt;
            Saldo = saldo;
        }
        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt) : this(bank, osoba, kredyt, 0) { }
        public Kredytowa(Bank bank, Osoba osoba, decimal kredyt, decimal saldo) : base(bank, osoba)
        {
            if (kredyt <= 0)
                throw new UjemnyKredyt(kredyt);
            Kredyt = kredyt;
            Saldo = saldo;
        }

        public override void Wplac(decimal kwota)
        {
            ZweryfikujKwote(kwota);
            Saldo += kwota;
        }
        public override bool Wyplac(decimal kwota)
        {
            if (!ZweryfikujKwote(kwota))
                throw new KwotaException(kwota);
            if (Saldo - kwota < -Kredyt) throw new WyplacException(kwota);

            Saldo -= kwota;
            return true;
        }

        internal class KredytowaJson : Json
        {
            public readonly decimal Kredyt;
            public readonly decimal Saldo;
            public readonly int Numer;

            public KredytowaJson(Kredytowa kredytowa) : base(kredytowa)
            {
                Kredyt = kredytowa.Kredyt;
                Saldo = kredytowa.Saldo;
                Numer = kredytowa.Numer;
            }
        }
        public override void Zapisz(string dir)
        {
            var json = JsonConvert.SerializeObject(new KredytowaJson(this), Json.JsonSerializerSettings);
            File.WriteAllText($"{dir}/{GetHashCode()}.json", json);
        }
    }
}
