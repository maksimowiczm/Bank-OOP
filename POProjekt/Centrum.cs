using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POProjekt
{
    public class Centrum
    {
        private readonly List<Transakcja> transakcje;
        private readonly List<Osoba> osoby;
        private readonly List<Firma> firmy;
        private readonly List<Bank> banki;
        public IList<Transakcja> Transakcje => transakcje.AsReadOnly();
        public IList<Osoba> Osoby => osoby.AsReadOnly();
        public IList<Firma> Firmy => firmy.AsReadOnly();
        public IList<Bank> Banki => banki.AsReadOnly();

        public Centrum() : this(new List<Transakcja>(), new List<Osoba>(), new List<Firma>(), new List<Bank>()) { }
        [JsonConstructor]
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

        /// <summary> Zapisuje całe centrum do pliku. </summary>
        /// <param name="nazwa">Nazwa pliku do którego ma zostać zapisane centrum.</param>
        public bool Zapisz(string nazwa)
        {
            var json = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.Objects,
            });
            File.WriteAllText($"{nazwa}.centrum", json);

            return true;
        }
        /// <summary> Odczytuje nazwy plików z których można wczytać centrum. </summary>
        /// <returns> Listę nazw plików które da się wczytać.</returns>
        public static List<string> Odczytaj()
        {
            var pliki = new List<string>();
            var plikiCentrum = Directory.GetFiles(Environment.CurrentDirectory, "*.centrum");
            foreach (var plik in plikiCentrum)
            {
                try
                {
                    Wczytaj(plik);
                    pliki.Add(plik);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return pliki;
        }
        /// <summary> Wczytuje centrum z pliku. </summary>
        /// <param name="nazwa">Nazwa pliku.</param>
        public static Centrum Wczytaj(string nazwa)
        {
            var json = "";
            try
            {
                json = File.ReadAllText($"{nazwa}.centrum");
            }
            catch (FileNotFoundException)
            {
                try
                {
                    json = File.ReadAllText(nazwa);
                }
                catch (FileNotFoundException)
                {
                    throw new NieMaPliku(nazwa);
                }
            }
            var wczytane = JsonConvert.DeserializeObject<Centrum>(json, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.Objects,
            });
            return wczytane;
        }
    }
}
