using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public class Kredytowa : Debetowa
    {
        public readonly decimal MaksymalnyKredyt;
        public decimal AktualnyKredyt => saldo >= 0 ? 0 : Math.Abs(Saldo);

        public Kredytowa(int idBanku, int idKlienta, decimal kredyt)
            : this(idBanku, idKlienta, kredyt, 0) { }
        public Kredytowa(int idBanku, int idKlienta, decimal kredyt, decimal saldo)
            : base(idBanku, idKlienta, 0)
        {
            if (kredyt <= 0)
                throw new Exception("Niedodatni kredyt");
            if (saldo < -kredyt)
                throw new Exception("Za niskie saldo");
            this.saldo = saldo;
            MaksymalnyKredyt = kredyt;
        }

        [JsonConstructor]
        public Kredytowa(int idBanku, int idKlienta, decimal kredyt, decimal saldo, string num)
            : base(idBanku, idKlienta, 0, num)
        {
            if (kredyt <= 0)
                throw new Exception("Niedodatni kredyt");
            if (saldo < -kredyt)
                throw new Exception("Za niskie saldo");
            this.saldo = saldo;
            MaksymalnyKredyt = kredyt;
        }

        public override bool Wyplac(decimal kwota)
        {
            if (!zweryfikujKwote(kwota))
                return false;

            if (saldo - kwota >= -MaksymalnyKredyt)
            {
                saldo -= kwota;
                return true;
            }
            else
                return false;
        }
    }
}
