﻿using System.Collections.Generic;

namespace POProjekt
{
    public abstract class Klient
    {
        protected List<Konto> konta = new List<Konto>();
        public IList<Konto> Konta => konta.AsReadOnly();

        public bool MojeKonto(Konto konto) => konta.Contains(konto);
        /// <summary>
        /// Dodaje konto jeśli jest tego klienta i nie ma już go na liście kont.
        /// </summary>
        public bool DodajKonto(Konto konto)
        {
            if (konto.Klient != this)
                throw new KontoNieIstnieje(konto, this);
            if (MojeKonto(konto))
                throw new KontoIstnieje(konto, this);

            konta.Add(konto);
            return true;
        }

        public abstract string ToString(string s);
    }
}
