namespace POProjekt
{
    public class Konto
    {
        public readonly Bank Bank;
        public readonly Klient Klient;
        private decimal saldo;

        public Konto(Bank bank, Klient klient) : this(bank, klient, 0) { }
        public Konto(Bank bank, Klient klient, decimal saldo)
        {
            Klient = klient;
            Bank = bank;
            this.saldo = saldo;
        }
    }
}
