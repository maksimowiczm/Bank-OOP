﻿using Newtonsoft.Json;
using System;

namespace POProjekt
{
    public class Transakcja
    {
        private static int ilosc = 0;
        public readonly int Id;
        public readonly DateTime Data;
        public readonly bool Sukces;
        public readonly Bank Bank;
        public readonly Firma Firma;
        public readonly Klient Klient;
        public readonly Karta Karta;
        public readonly decimal Kwota;
        
        [JsonConstructor]
        public Transakcja(int id, DateTime data, bool sukces, Bank bank, Firma firma, Klient klient, Karta karta, decimal kwota)
        {
            Id = id;
            Data = data;
            Sukces = sukces;
            Bank = bank;
            Firma = firma;
            Klient = klient;
            Karta = karta;
            Kwota = kwota;
        }
        public Transakcja(DateTime data, bool sukces, Bank bank, Firma firma, Klient klient, Karta karta, decimal kwota)
            : this(ilosc++, data, sukces, bank, firma, klient, karta, kwota) { }
    }
}
