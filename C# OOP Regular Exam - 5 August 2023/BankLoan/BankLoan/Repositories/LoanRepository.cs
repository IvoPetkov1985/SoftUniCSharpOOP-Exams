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

        public IReadOnlyCollection<ILoan> Models
            => loans.AsReadOnly();

        public void AddModel(ILoan loan)
        {
            loans.Add(loan);
        }

        public ILoan FirstModel(string name)
        {
            return loans.FirstOrDefault(l => l.GetType().Name == name);
        }

        public bool RemoveModel(ILoan loan)
        {
            return loans.Remove(loan);
        }
    }
}
