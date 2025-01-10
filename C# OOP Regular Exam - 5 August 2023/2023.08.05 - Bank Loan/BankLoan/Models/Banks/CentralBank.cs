namespace BankLoan.Models.Banks
{
    public class CentralBank : Bank
    {
        private const int CentralBankCapacity = 50;

        public CentralBank(string name)
            : base(name, CentralBankCapacity)
        {
        }
    }
}
