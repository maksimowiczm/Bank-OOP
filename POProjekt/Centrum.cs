using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Centrum
    {
        public static bool wczytywanie { get; private set; }

        private readonly List<Transakcja> transakcje;
        private readonly List<Osoba> osoby;
        private readonly List<Firma> firmy;
        private readonly List<Bank> banki;
        public IList<Transakcja> Transakcje => transakcje.AsReadOnly();
        public IList<Osoba> Osoby => osoby.AsReadOnly();
        public IList<Firma> Firmy => firmy.AsReadOnly();
        public IList<Bank> Banki => banki.AsReadOnly();

        public Centrum() : this(new List<Transakcja>(), new List<Osoba>(), new List<Firma>(), new List<Bank>()) { }

        public Centrum(List<Transakcja> transakcje, List<Osoba> osoby, List<Firma> firmy, List<Bank> banki)
        {
            this.transakcje = transakcje;
            this.osoby = osoby;
            this.firmy = firmy;
            this.banki = banki;
        }

        /// <summary> Prosi bank o realizację transakcji i dodaję ją do listy transakcji. </summary>
        /// <returns> Sukces transakcji. </returns>
        public bool AutoryzujTransakcje(Firma firma, Karta karta, decimal kwota)
        {
            if (kwota <= 0)
                throw new KwotaException(kwota);

            var bank = karta.Bank;
            var sukces = bank.RealizujTransakcje(karta, kwota);
            var transakcja = new Transakcja(DateTime.Now, sukces, bank, firma, karta.Osoba, karta, kwota);
            transakcje.Add(transakcja);
            return sukces;
        }

        /// <summary> Dzieli podany string (wyrazy odzielone spacją) na Listę stringów. "osoba and karta and firma" => {osoba, and, karta, and, firma}. </summary>
        private static List<string> Podziel(string zapytanie)
        {
            var lista = new List<string>();
            var wyraz = "";
            zapytanie = zapytanie.ToLower();
            foreach (var ch in zapytanie.Where(ch => ch != ' '))
            {
                wyraz += ch;
                if (wyraz.Length > 5)
                    throw new ZapytanieException(zapytanie, wyraz);
                switch (wyraz)
                {
                    case "osoba":
                    case "firma":
                    case "bank":
                    case "data":
                    case "kwota":
                    case "karta":
                    case "and":
                    case "or":
                        lista.Add(wyraz);
                        wyraz = "";
                        break;
                }
            }

            //sprawdza czy zapytanie jest dobrze połączone and/or
            for (var i = 0; i < lista.Count; i++)
            {
                if (i % 2 == 1)
                    switch (lista[i])
                    {
                        case "and":
                        case "or":
                            continue;
                        default:
                            throw new ZapytanieException(zapytanie, lista);
                    }
                switch (lista[i])
                {
                    case "osoba":
                    case "firma":
                    case "bank":
                    case "data":
                    case "kwota":
                    case "karta":
                        continue;
                    default:
                        throw new ZapytanieException(zapytanie, lista);
                }
            }

            return lista;
        }

        /// <summary> Zwraca listę transakcji, które posiadają podany obiekt. </summary>
        private static List<Transakcja> Znajdz(object obj, List<Transakcja> transakcje)
        {
            var lista = new List<Transakcja>();

            lista.AddRange(transakcje.Where(t => t.Osoba.Equals(obj)));
            lista.AddRange(transakcje.Where(t => t.Firma.Equals(obj)));
            lista.AddRange(transakcje.Where(t => t.Bank.Equals(obj)));
            lista.AddRange(transakcje.Where(t => t.Karta.Equals(obj)));
            lista.AddRange(transakcje.Where(t => obj is DateTime data && (t.Data.Day == data.Day && t.Data.Month == data.Month && t.Data.Year == data.Year)));
            lista.AddRange(transakcje.Where(t => t.Kwota.Equals(obj)));

            return lista;
        }

        /// <summary> Zamienia podanego stringa w obiekt z Zapytania. </summary>
        private static object Obiektuj(string wyraz, Zapytanie req)
        {
            return wyraz switch
            {
                "osoba" => req.Osoba,
                "firma" => req.Firma,
                "bank" => req.Bank,
                "data" => req.Data,
                "kwota" => req.Kwota,
                "karta" => req.Karta,
                _ => throw new ZapytanieException(req.Pytanie)
            };
        }

        /// <summary> Zamienia Listę stringów w pary. Pomija pierwszy element. Używana do zaawansowanego Zapytania. { osoba, and, klient, or, firma } => { (and, klient), (or, firma) } </summary>
        private static List<Pair> Paruj(List<string> zapytanie, Zapytanie req)
        {
            var pary = new List<Pair>();
            for (var i = 2; i < zapytanie.Count; i += 2)
                pary.Add(new Pair(zapytanie[i - 1], Obiektuj(zapytanie[i], req)));

            return pary;
        }

        /// <summary> Znajduje i zwraca listę transakcji, które spełniają podane warunki. </summary>
        public List<Transakcja> ZnajdzTransakcje(Zapytanie req)
        {
            var zapytanie = Podziel(req.Pytanie);

            //Tworzy początkową listę transakcji na podstawie pierwszego wyrazu z zapytania.
            var obj = Obiektuj(zapytanie[0], req);
            var lista = Znajdz(obj, transakcje);

            if (zapytanie.Count <= 2) return lista;

            var pary = Paruj(zapytanie, req);
            foreach (var pair in pary)
            {
                //W zależności od połączenia zapytania (or/and) dodaje transakcje do listy.
                switch (pair.Andor)
                {
                    case "or":
                        lista.AddRange(Znajdz(pair.Obj, transakcje));
                        break;
                    case "and":
                        lista = Znajdz(pair.Obj, lista);
                        break;
                }
                //Usuwa duplikaty transakcji z listy.
                lista = lista.Distinct().ToList();
            }

            return lista;
        }

        public void DodajOsobe(Osoba osoba) => osoby.Add(osoba);
        public void DodajFirme(Firma firma) => firmy.Add(firma);
        public void DodajBank(Bank bank) => banki.Add(bank);

        /// <summary> Wczytuje centrum z dysku. </summary>
        /// <param name="nazwa"> Nazwa folderu. </param>
        public static Centrum Wczytaj(string nazwa)
        {
            if (!Directory.Exists(nazwa))
                throw new NieMaPliku(nazwa);

            wczytywanie = true;
            var Ddebetowa = $"{nazwa}/karty/debetowa";
            var Dkredytowa = $"{nazwa}/karty/kredytowa";
            var Dkonta = $"{nazwa}/konta";
            var Dbanki = $"{nazwa}/banki";
            var Dosoby = $"{nazwa}/osoby";
            var Dfirmy = $"{nazwa}/firmy";

            var konta = Directory.GetFiles(Dkonta);
            var kontoDic = konta.ToDictionary(s => Konto.Wczytaj(s).Hash, Konto.Wczytaj);

            var debetowe = Directory.GetFiles(Ddebetowa);
            var debetowaDic = debetowe.ToDictionary(s => Debetowa.Wczytaj(s).Hash, Debetowa.Wczytaj);

            var kredytowe = Directory.GetFiles(Dkredytowa);
            var kredytowaDic = kredytowe.ToDictionary(s => Kredytowa.Wczytaj(s).Hash, Kredytowa.Wczytaj);

            var banki = Directory.GetFiles(Dbanki);
            var bankDic = banki.ToDictionary(s => Bank.Wczytaj(s).Hash, Bank.Wczytaj);

            var osoby = Directory.GetFiles(Dosoby);
            var osobaDic = osoby.ToDictionary(s => Osoba.Wczytaj(s).Hash, Osoba.Wczytaj);

            var firmy = Directory.GetFiles(Dfirmy);
            var firmaDic = firmy.ToDictionary(s => Firma.Wczytaj(s).Hash, Firma.Wczytaj);

            var RealBanki = bankDic.Values.ToDictionary(bank => bank.Hash, bank => new Bank(bank.Nazwa));
            var RealOsoby = osobaDic.Values.ToDictionary(osoba => osoba.Hash, osoba => new Osoba(osoba.Imie, osoba.Nazwisko));
            var RealFirmy = firmaDic.Values.ToDictionary(firma => firma.Hash, firma => new Firma(firma.Nazwa, firma.Kategoria, new Centrum()));


            var RealKonta = new Dictionary<int, Konto>();
            foreach (var konto in kontoDic.Values)
            {
                var bank = RealBanki[konto.BankHash];
                Klient klient;
                if (RealOsoby.ContainsKey(konto.KlientHash))
                    klient = RealOsoby[konto.KlientHash];
                else if (RealFirmy.ContainsKey(konto.KlientHash))
                    klient = RealFirmy[konto.KlientHash];
                else
                    throw new Exception();

                var RealKonto = new Konto(bank, klient, konto.Saldo);
                klient.DodajKonto(RealKonto);
                bank.DodajKonto(RealKonto);
                RealKonta.Add(konto.Hash, RealKonto);
            }

            foreach (var debetowa in debetowaDic.Values)
            {
                var bank = RealBanki[debetowa.BankHash];
                var osoba = RealOsoby[debetowa.OsobaHash];
                var konto = RealKonta[debetowa.KontoHash];
                var RealDebetowa = new Debetowa(bank, osoba, konto, debetowa.Numer);
                osoba.DodajKarte(RealDebetowa);
                bank.DodajKarte(RealDebetowa);
            }

            foreach (var kredytowa in kredytowaDic.Values)
            {
                var bank = RealBanki[kredytowa.BankHash];
                var osoba = RealOsoby[kredytowa.OsobaHash];
                var RealKredytowa = new Kredytowa(bank, osoba, kredytowa.Kredyt, kredytowa.Saldo, kredytowa.Numer);
                osoba.DodajKarte(RealKredytowa);
                bank.DodajKarte(RealKredytowa);
            }

            var ListOsoby = RealOsoby.Values.ToList();
            var ListBanki = RealBanki.Values.ToList();
            var ListFirmy = RealFirmy.Values.ToList();

            var centrum = new Centrum(new List<Transakcja>(), ListOsoby, ListFirmy, ListBanki);
            foreach (var firma in centrum.firmy)
                firma.Centrum = centrum;

            wczytywanie = false;
            return centrum;
        }

        /// <summary> Zapisuje całe centrum na dysk. </summary>
        /// <param name="nazwa">Nazwa folderu do którego ma zostać zapisane centrum.</param>
        public bool Zapisz(string nazwa)
        {
            var Dkarty = $"{nazwa}/karty";
            var Dkonta = $"{nazwa}/konta";
            var Dbanki = $"{nazwa}/banki";
            var Dosoby = $"{nazwa}/osoby";
            var Dfirmy = $"{nazwa}/firmy";
            if (Directory.Exists(nazwa))
                Directory.Delete(nazwa, true);

            Directory.CreateDirectory(Dkarty);
            Directory.CreateDirectory($"{Dkarty}/debetowa");
            Directory.CreateDirectory($"{Dkarty}/kredytowa");
            Directory.CreateDirectory(Dkarty);
            Directory.CreateDirectory(Dkonta);
            Directory.CreateDirectory(Dbanki);
            Directory.CreateDirectory(Dosoby);
            Directory.CreateDirectory(Dfirmy);
            foreach (var bank in banki)
            {
                foreach (var karta in bank.Karty)
                    karta.Zapisz(Dkarty);

                foreach (var konto in bank.Konta)
                    konto.Zapisz(Dkonta);

                bank.Zapisz(Dbanki);
            }
            foreach (var osoba in osoby)
                osoba.Zapisz(Dosoby);

            foreach (var firma in firmy)
                firma.Zapisz(Dfirmy);


            var transakcjeJson = transakcje.Select(transakcja => transakcja.Json()).Cast<object>().ToList();
            File.WriteAllText($"{nazwa}/czytelneTransakcje.json", JsonConvert.SerializeObject(transakcjeJson, Json.JsonSerializerSettings));

            return true;
        }
    }
}
