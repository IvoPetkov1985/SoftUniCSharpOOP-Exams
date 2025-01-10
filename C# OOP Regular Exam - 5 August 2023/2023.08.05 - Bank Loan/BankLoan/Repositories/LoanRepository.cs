using BankLoan.Models.Contracts;
using BankLoan.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BankLoan.Repositories
{
    public class LoanRepository : IRepository<ILoan>
    {
        private readonly List<ILoan> loans;

        public LoanRepository()
        {
            loans = new List<ILoan>();
        }

        public IReadOnlyCollection<ILoan> Models => loans.AsReadOnly();

        public void AddModel(ILoan loan)
        {
            loans.Add(loan);
        }

        public ILoan FirstModel(string typeName)
        {
            ILoan loan = loans.FirstOrDefault(l => l.GetType().Name == typeName);
            return loan;
        }

        public bool RemoveModel(ILoan loan)
        {
            return loans.Remove(loan);
        }
    }
}
