using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public static class Centrum
    {
        private static List<Transakcja> transakcje = new List<Transakcja>();
        public static List<Transakcja> Transakcje { get => transakcje; }

        public static bool AutoryzujTransakcje(Firma firma, Karta karta, decimal kwota)
        {
            var bank = Bank.GetBank(karta.IdBanku);
            if (bank == null)
                return false;

            var sukces = bank.RealizujTransakcje(karta, kwota);
            var transakcja = new Transakcja(DateTime.Now, sukces, bank.Id, bank.Nazwa, firma.Nazwa, karta.IdKlienta, karta.Numer, kwota);
            transakcje.Add(transakcja);
            return sukces;
        }
        public static void ZapiszTransakcje(string uri)
        {
            var json = JsonConvert.SerializeObject(transakcje, Formatting.Indented);
            File.WriteAllText($"{uri}.json", json);
        }
        
        private static string getName(string zapytanie, ref int from)
        {
            var i = from;
            var length = zapytanie.Length;
            if (i >= length)
                return null;

            while (zapytanie[i] != '"')
                i++;
            i++;
            var wyraz = "";
            while (zapytanie[i] != '"')
                wyraz += zapytanie[i++];
            from = i;
            return wyraz;
        }
        private static List<Transakcja> UsunDuplikaty(List<Transakcja> lista)
        {
            var bylo = new bool[transakcje.Count];
            var nowa = new List<Transakcja>();

            foreach (var transakcja in lista.Where(transakcja => bylo[transakcja.Id] == false))
            {
                nowa.Add(transakcja);
                bylo[transakcja.Id] = true;
            }
            return nowa;
        }
        public static List<Transakcja> ZnajdzTransakcje(string zapytanie)
        {
            var connetors = new List<string>();
            var pytanie = new Dictionary<string, string>();
            var pokolei = new List<string>();

            var wyraz = "";
            for (var i = 0; i < zapytanie.Length; i++)
            {
                var znak = zapytanie[i];
                if (znak != ' ')
                {
                    wyraz += znak;
                    switch (wyraz)
                    {
                        case "Firma":
                        case "Bank":
                        case "Num":
                        case "Klient":
                        case "Kwota":
                            pokolei.Add(wyraz);
                            pytanie.Add(wyraz, getName(zapytanie, ref i));
                            wyraz = "";
                            break;
                        case "AND":
                        case "OR":
                            connetors.Add(wyraz);
                            wyraz = "";
                            break;
                    }
                }
            }
            
            var aktualna = new List<Transakcja>();
            switch (pokolei[0])
            {
                case "Firma":
                    aktualna = transakcje.FindAll(t => t.NazwaFirmy == pytanie["Firma"]);
                    break;
                case "Bank":
                    aktualna = transakcje.FindAll(t => t.NazwaBanku == pytanie["Bank"]);
                    break;
                case "Num":
                    aktualna = transakcje.FindAll(t => t.NumKarty == pytanie["Num"]);
                    break;
                case "Klient":
                    aktualna = transakcje.FindAll(t => t.IdKLienta == int.Parse(pytanie["Klient"]));
                    break;
                case "Kwota":
                    aktualna = transakcje.FindAll(t => t.Kwota == decimal.Parse(pytanie["Kwota"]));
                    break;
            }

            for (var i = 1; i < pokolei.Count; i++)
            {
                if (connetors[i - 1] == "AND")
                    aktualna = pokolei[i] switch
                    {
                        "Firma" => aktualna.FindAll(t => t.NazwaFirmy == pytanie["Firma"]),
                        "Bank" => aktualna.FindAll(t => t.NazwaBanku == pytanie["Bank"]),
                        "Num" => aktualna.FindAll(t => t.NumKarty == pytanie["Num"]),
                        "Klient" => aktualna.FindAll(t => t.IdKLienta == int.Parse(pytanie["Klient"])),
                        "Kwota" => aktualna.FindAll(t => t.Kwota == decimal.Parse(pytanie["Kwota"])),
                        _ => aktualna
                    };
                else if (connetors[i - 1] == "OR")
                {
                    var nowe = new List<Transakcja>();
                    if(pokolei[i] == "Firma")
                        nowe = transakcje.FindAll(t => t.NazwaFirmy == pytanie["Firma"]);
                    else if(pokolei[i]=="Bank")
                        nowe = transakcje.FindAll(t => t.NazwaBanku == pytanie["Bank"]);
                    else if(pokolei[i]=="Num")
                        nowe = transakcje.FindAll(t => t.NumKarty == pytanie["Num"]);
                    else if(pokolei[i]=="Klient")
                        nowe = transakcje.FindAll(t => t.IdKLienta == int.Parse(pytanie["Klient"]));
                    else if(pokolei[i]=="Kwota")
                        nowe = transakcje.FindAll(t => t.Kwota == decimal.Parse(pytanie["Kwota"]));
                    aktualna.AddRange(nowe);
                    aktualna = UsunDuplikaty(aktualna);
                }
            }
            return aktualna;
        }
    }
}
