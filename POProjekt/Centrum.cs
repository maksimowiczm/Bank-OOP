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

        public Karta GetKarta(int numer)
        {
            foreach (var bank in Banki)
                foreach (var karta in bank.Karty)
                    if (numer == karta.Numer)
                        return karta;
            throw new KartaNieIstnieje(null, this);
        }

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
            var transakcja = new Transakcja(DateTime.Now, sukces, kwota, firma, karta.Osoba, karta);
            transakcje.Add(transakcja);

            if (!sukces) return false;

            try
            {
                firma.Konta[0].Wplac(kwota);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FirmaException(firma, "Firma nie posiada konta");
            }

            return true;
        }

        private static List<string> Podziel(string zapytanie)
        {
            var lista = new List<string>();

            var wyraz = "";
            zapytanie = zapytanie.ToLower();
            foreach (var ch in zapytanie.Where(ch => ch != ' '))
            {
                wyraz += ch;
                if (wyraz.Length > 5)
                    throw new ZapytanieException(zapytanie);
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

            return lista;
        }

        private List<Transakcja> Znajdz(string s, List<Transakcja> lista, Zapytanie req)
        {
            var nowaLista = new List<Transakcja>();
            switch (s)
            {
                case "osoba":
                    nowaLista.AddRange(lista.Where(t => t.Osoba == req.Osoba.ToString("j")));
                    break;
                case "firma":
                    nowaLista.AddRange(lista.Where(t => t.Firma == req.Firma.ToString("j")));
                    break;
                case "bank":
                    nowaLista.AddRange(lista.Where(t => t.BankOsoby == req.Bank.ToString("j")));
                    nowaLista.AddRange(lista.Where(t => t.BankFirmy == req.Bank.ToString("j")));
                    break;
                case "data":
                    var data = req.Data ?? throw new ZapytanieException(req);
                    nowaLista.AddRange(lista.Where(t =>
                        t.Data.Day == data.Day && t.Data.Month == data.Month && t.Data.Year == data.Year));
                    break;
                case "kwota":
                    nowaLista.AddRange(lista.Where(t => t.Kwota == req.Kwota));
                    break;
                case "karta":
                    nowaLista.AddRange(lista.Where(t => t.Karta == req.Karta.Numer));
                    break;
            }

            return nowaLista;
        }

        public List<Transakcja> ZnajdzTransakcje(Zapytanie req)
        {
            var zapytanieStrings = Podziel(req.Pytanie);
            var aktualneTransakcje = Znajdz(zapytanieStrings[0], transakcje, req);

            for (var i = 2; i <= zapytanieStrings.Count; i += 2)
            {
                var s = zapytanieStrings[i - 1];
                switch (s)
                {
                    case "and":
                        aktualneTransakcje = Znajdz(zapytanieStrings[i], aktualneTransakcje, req);
                        break;
                    case "or":
                        aktualneTransakcje.AddRange(Znajdz(zapytanieStrings[i], transakcje, req));
                        break;
                    default:
                        throw new ZapytanieException(req);
                }

                if (transakcje.Count == 0)
                    return aktualneTransakcje;
            }

            aktualneTransakcje = aktualneTransakcje.Distinct().ToList();

            return aktualneTransakcje;
        }

        public void DodajOsobe(Osoba osoba) => osoby.Add(osoba);
        public void DodajFirme(Firma firma) => firmy.Add(firma);
        public void DodajBank(Bank bank) => banki.Add(bank);

        /// <summary> Wczytuje centrum z dysku. </summary>
        /// <param name="nazwa"> Nazwa pliku. </param>
        public static Centrum Wczytaj(string nazwa)
        {
            wczytywanie = true;
            Zapis zapis;
            try
            {
                var json = File.ReadAllText($"{nazwa}.json");
                zapis = JsonConvert.DeserializeObject<Zapis>(json);
            }
            catch (Exception)
            {
                throw new WczytwanieZapisException(nazwa);
            }

            var kontoDic = zapis.Konta.ToDictionary(o => o.Hash, o => o);
            var debetowaDic = zapis.Debetowe.ToDictionary(o => o.Hash, o => o);
            var kredytowaDic = zapis.Kredytowe.ToDictionary(o => o.Hash, o => o);
            var bankDic = zapis.Banki.ToDictionary(o => o.Hash, o => o);
            var osobaDic = zapis.Osoby.ToDictionary(o => o.Hash, o => o);
            var firmaDic = zapis.Firmy.ToDictionary(o => o.Hash, o => o);

            var bankiObj = bankDic.Values.ToDictionary(bank => bank.Hash, bank => new Bank(bank.Nazwa));
            var osobyObj = osobaDic.Values.ToDictionary(osoba => osoba.Hash, osoba => new Osoba(osoba.Imie, osoba.Nazwisko));
            var firmyObj = firmaDic.Values.ToDictionary(firma => firma.Hash, firma => new Firma(firma.Nazwa, firma.Kategoria, new Centrum()));
            var kontaObj = new Dictionary<int, Konto>();

            //Tworzenie kont z referencjami do odpowiednich obiektów.
            foreach (var konto in kontoDic.Values)
            {
                try
                {
                    var bank = bankiObj[konto.BankHash];
                    Klient klient;
                    if (firmyObj.ContainsKey(konto.KlientHash))
                        klient = firmyObj[konto.KlientHash];
                    else if (osobyObj.ContainsKey(konto.KlientHash))
                        klient = osobyObj[konto.KlientHash];
                    else
                        throw new Exception();

                    var realKonto = new Konto(bank, klient, konto.Saldo);
                    klient.DodajKonto(realKonto);
                    bank.DodajKonto(realKonto);
                    kontaObj.Add(konto.Hash, realKonto);
                }
                catch (Exception e)
                {
                    throw new DeserializacjaException<KontoJson>(nazwa, konto.KlientHash, kontoDic, e);
                }
            }

            //Tworzenie kart z referencjami do odpowiednich obiektów.
            foreach (var debetowa in debetowaDic.Values)
            {
                try
                {
                    var bank = bankiObj[debetowa.BankHash];
                    var osoba = osobyObj[debetowa.OsobaHash];
                    var konto = kontaObj[debetowa.KontoHash];
                    var realDebetowa = new Debetowa(bank, osoba, konto, debetowa.Numer);
                    osoba.DodajKarte(realDebetowa);
                    bank.DodajKarte(realDebetowa);
                }
                catch (Exception e)
                {
                    throw new DeserializacjaException<DebetowaJson>(nazwa, debetowa, debetowaDic, e);
                }
            }
            foreach (var kredytowa in kredytowaDic.Values)
            {
                try
                {
                    var bank = bankiObj[kredytowa.BankHash];
                    var osoba = osobyObj[kredytowa.OsobaHash];
                    var realKredytowa = new Kredytowa(bank, osoba, kredytowa.Kredyt, kredytowa.Saldo, kredytowa.Numer);
                    osoba.DodajKarte(realKredytowa);
                    bank.DodajKarte(realKredytowa);
                }
                catch (Exception e)
                {
                    throw new DeserializacjaException<KredytowaJson>(nazwa, kredytowa, kredytowaDic, e);
                }
            }

            //Zamiana Directory na listy
            var listOsoby = osobyObj.Values.ToList();
            var listBanki = bankiObj.Values.ToList();
            var listFirmy = firmyObj.Values.ToList();

            var centrum = new Centrum(zapis.Transakcje, listOsoby, listFirmy, listBanki);
            //Wstawienie odpowiedniego centrum do firmy
            foreach (var firma in centrum.firmy)
                firma.Centrum = centrum;

            wczytywanie = false;
            return centrum;
        }

        /// <summary> Zapisuje całe centrum na dysk. </summary>
        /// <param name="nazwa">Nazwa pliku do którego ma zostać zapisane centrum.</param>
        public bool Zapisz(string nazwa)
        {
            var zapis = new Zapis()
            {
                Banki = banki.Select(b => b.makeJson()).ToList(),
                Osoby = osoby.Select(b => b.makeJson()).ToList(),
                Firmy = firmy.Select(b => b.makeJson()).ToList(),
                Transakcje = transakcje,
                Kredytowe = new List<KredytowaJson>(),
                Debetowe = new List<DebetowaJson>(),
                Konta = new List<KontoJson>(),
            };

            foreach (var bank in banki)
            {
                foreach (var karta in bank.Karty)
                {
                    if (karta.GetType() == typeof(Kredytowa))
                        zapis.Kredytowe.Add((karta as Kredytowa)?.makeJson());
                    else
                        zapis.Debetowe.Add((karta as Debetowa)?.makeJson());
                }

                foreach (var konto in bank.Konta)
                    zapis.Konta.Add(konto.makeJson());
            }

            zapis.Zapisz($"{nazwa}.json");
            return true;
        }
    }
}
