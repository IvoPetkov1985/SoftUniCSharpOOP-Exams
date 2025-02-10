using BankLoan.Core.Contracts;
using BankLoan.Models.Banks;
using BankLoan.Models.Clients;
using BankLoan.Models.Contracts;
using BankLoan.Models.Loans;
using BankLoan.Repositories;
using BankLoan.Repositories.Contracts;
using System;
using System.Linq;
using System.Text;

namespace BankLoan.Core
{
    public class Controller : IController
    {
        private readonly IRepository<ILoan> loans;
        private readonly IRepository<IBank> banks;

        public Controller()
        {
            loans = new LoanRepository();
            banks = new BankRepository();
        }

        public string AddBank(string bankTypeName, string name)
        {
            IBank bank = null;

            if (bankTypeName == nameof(BranchBank))
            {
                bank = new BranchBank(name);
            }
            else if (bankTypeName == nameof(CentralBank))
            {
                bank = new CentralBank(name);
            }
            else
            {
                throw new ArgumentException("Invalid bank type.");
            }

            banks.AddModel(bank);
            return $"{bankTypeName} is successfully added.";
        }

        public string AddClient(string bankName, string clientTypeName, string clientName, string id, double income)
        {
            IClient client = null;

            if (clientTypeName == nameof(Student))
            {
                client = new Student(clientName, id, income);
            }
            else if (clientTypeName == nameof(Adult))
            {
                client = new Adult(clientName, id, income);
            }
            else
            {
                throw new ArgumentException("Invalid client type.");
            }

            IBank bank = banks.FirstModel(bankName);

            if ((clientTypeName == nameof(Student) && bank.GetType().Name == nameof(CentralBank)) ||
                (clientTypeName == nameof(Adult) && bank.GetType().Name == nameof(BranchBank)))
            {
                return "Unsuitable bank.";
            }

            bank.AddClient(client);
            return $"{clientTypeName} successfully added to {bankName}.";
        }

        public string AddLoan(string loanTypeName)
        {
            ILoan loan = null;

            if (loanTypeName == nameof(MortgageLoan))
            {
                loan = new MortgageLoan();
            }
            else if (loanTypeName == nameof(StudentLoan))
            {
                loan = new StudentLoan();
            }
            else
            {
                throw new ArgumentException("Invalid loan type.");
            }

            loans.AddModel(loan);
            return $"{loanTypeName} is successfully added.";
        }

        public string FinalCalculation(string bankName)
        {
            IBank bank = banks.FirstModel(bankName);
            double funds = bank.Clients.Sum(c => c.Income) + bank.Loans.Sum(l => l.Amount);
            return $"The funds of bank {bankName} are {funds:F2}.";
        }

        public string ReturnLoan(string bankName, string loanTypeName)
        {
            IBank bank = banks.FirstModel(bankName);
            ILoan loan = loans.FirstModel(loanTypeName) ??
                throw new ArgumentException($"Loan of type {loanTypeName} is missing.");

            bank.AddLoan(loan);
            loans.RemoveModel(loan);
            return $"{loanTypeName} successfully added to {bankName}.";
        }

        public string Statistics()
        {
            StringBuilder builder = new();

            foreach (IBank bank in banks.Models)
            {
                builder.Append(bank.GetStatistics());
            }

            return builder.ToString().TrimEnd();
        }
    }
}
