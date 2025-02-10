namespace BankLoan.Models.Loans
{
    public class StudentLoan : Loan
    {
        private const int StudentLoanIntRate = 1;
        private const double StudentLoanAmount = 10000;

        public StudentLoan()
            : base(StudentLoanIntRate, StudentLoanAmount)
        {
        }
    }
}
