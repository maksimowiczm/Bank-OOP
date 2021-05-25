using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
        public List<Transakcja> ZnajdzTransakcje(string zapytanie)
        {
            throw new NotImplementedException();
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
