using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POProjekt
{
    public class Debetowa : Karta
    {
        public Debetowa(int idBanku, int idKlienta)
            :base(idBanku, idKlienta) { }
        public Debetowa(int idBanku, int idKlienta, decimal saldo)
            :base(idBanku, idKlienta, saldo) { }
        public Debetowa(int idBanku, int idKlienta, decimal saldo, string num)
            :base(idBanku, idKlienta, saldo, num) { }

        public override bool Wyplac(decimal kwota)
        {
            if (!zweryfikujKwote(kwota))
                return false;

            if (kwota <= saldo)
            {
                saldo -= kwota;
                return true;
            }
            else
                return false;
        }
    }
}
