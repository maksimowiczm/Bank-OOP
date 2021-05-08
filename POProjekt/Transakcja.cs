using System;
using Newtonsoft.Json;

namespace POProjekt
{
    public class Transakcja
    {
        public readonly DateTime Data;
        public readonly bool Sukces;
        public readonly int IdBanku;
        public readonly string NazwaBanku;
        public readonly int IdKLienta;
        public readonly string NumKarty;
        public readonly decimal Kwota;

        [JsonConstructor]
        public Transakcja(DateTime data, bool sukces, int idBanku, string nazwaBanku, int idKLienta, string numKarty, decimal kwota)
        {
            Data = data;
            Sukces = sukces;
            IdBanku = idBanku;
            NazwaBanku = nazwaBanku;
            IdKLienta = idKLienta;
            NumKarty = numKarty;
            Kwota = kwota;
        }
    }
}
