using BankLoan.Models.Contracts;
using BankLoan.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BankLoan.Repositories
{
    public class BankRepository : IRepository<IBank>
    {
        private readonly List<IBank> banks;

        public BankRepository()
        {
            banks = new List<IBank>();
        }

        public IReadOnlyCollection<IBank> Models
            => banks.AsReadOnly();

        public void AddModel(IBank bank)
        {
            banks.Add(bank);
        }

        public IBank FirstModel(string name)
        {
            return banks.FirstOrDefault(b => b.Name == name);
        }

        public bool RemoveModel(IBank bank)
        {
            return banks.Remove(bank);
        }
    }
}
