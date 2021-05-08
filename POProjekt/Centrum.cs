using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace POProjekt
{
    public static class Centrum
    {
        private static List<Transakcja> transakcje;
        public static List<Transakcja> Transakcje{ get => transakcje; }

        public static bool AutoryzujTransakcje(Firma firma, Karta karta, decimal kwota)
        {
            var bank = Bank.GetBank(karta.IdBanku);
            if (bank == null)
                return false;

            var sukces = bank.RealizujTransakcje(karta, kwota);
            var transakcja = new Transakcja(DateTime.Now, sukces, bank.Id, bank.Nazwa, karta.IdKlienta, karta.Numer, kwota);
            transakcje.Add(transakcja);
            return sukces;
        }
    }
}
