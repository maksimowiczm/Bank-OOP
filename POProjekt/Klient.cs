using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POProjekt
{
	public abstract class Klient
    {
        protected List<Konto> konta;
        public IList<Konto> Konta => konta.AsReadOnly();
    }
}
