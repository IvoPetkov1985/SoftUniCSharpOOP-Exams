namespace BankLoan.Models.Loans
{
    public class MortgageLoan : Loan
    {
        private const int MortgageLoanIntRate = 3;
        private const double MortgageLoanAmount = 50000;

        public MortgageLoan()
            : base(MortgageLoanIntRate, MortgageLoanAmount)
        {
        }
    }
}
