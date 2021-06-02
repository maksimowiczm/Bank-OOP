using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace POProjekt
{
    public class Zapis
    {
        public List<Kredytowa.KredytowaJson> Kredytowe { get; init; }
        public List<Debetowa.DebetowaJson> Debetowe { get; init; }
        public List<Konto.KontoJson> Konta { get; init; }
        public List<Bank.BankJson> Banki { get; init; }
        public List<Osoba.OsobaJson> Osoby { get; init; }
        public List<Firma.FirmaJson> Firmy { get; init; }
        public List<Transakcja.TransakcjaJson> Transakcje { get; init; }

        public void Zapisz(string plik)
        {
            File.WriteAllText(plik, JsonConvert.SerializeObject(this, Json.JsonSerializerSettings));
        }
    }
}
