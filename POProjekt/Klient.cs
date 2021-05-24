using System.Collections.Generic;

namespace POProjekt
{
    public abstract class Klient
    {
        protected List<Konto> konta = new List<Konto>();
        public IList<Konto> Konta => konta.AsReadOnly();

        public bool DodajKonto(Konto konto)
        {
            if (konto.Klient != this || konta.Contains(konto)) return false;

            konta.Add(konto);
            return true;
        }

        public bool UsunKonto(Konto konto)
        {
            if (konto.Klient != this || konta.Contains(konto)) return false;

            konta.Remove(konto);
            return true;
        }
    }
}
