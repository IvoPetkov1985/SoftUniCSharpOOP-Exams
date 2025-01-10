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
            if (bankTypeName != nameof(CentralBank) && bankTypeName != nameof(BranchBank))
            {
                throw new ArgumentException("Invalid bank type.");
            }

            IBank bank = null;

            if (bankTypeName == nameof(CentralBank))
            {
                bank = new CentralBank(name);
            }
            else
            {
                bank = new BranchBank(name);
            }

            banks.AddModel(bank);
            return $"{bankTypeName} is successfully added.";
        }

        public string AddClient(string bankName, string clientTypeName, string clientName, string id, double income)
        {
            if (clientTypeName != nameof(Adult) && clientTypeName != nameof(Student))
            {
                throw new ArgumentException("Invalid client type.");
            }

            IClient client = null;

            if (clientTypeName == nameof(Adult))
            {
                client = new Adult(clientName, id, income);
            }
            else
            {
                client = new Student(clientName, id, income);
            }

            IBank bank = banks.FirstModel(bankName);

            if ((bank.GetType().Name == nameof(BranchBank) && clientTypeName != nameof(Student))
                || bank.GetType().Name == nameof(CentralBank) && clientTypeName != nameof(Adult))
            {
                return "Unsuitable bank.";
            }

            bank.AddClient(client);
            return $"{clientTypeName} successfully added to {bankName}.";
        }

        public string AddLoan(string loanTypeName)
        {
            if (loanTypeName != nameof(StudentLoan) && loanTypeName != nameof(MortgageLoan))
            {
                throw new ArgumentException("Invalid loan type.");
            }

            ILoan loan = null;

            if (loanTypeName == nameof(StudentLoan))
            {
                loan = new StudentLoan();
            }
            else
            {
                loan = new MortgageLoan();
            }

            loans.AddModel(loan);
            return $"{loanTypeName} is successfully added.";
        }

        public string FinalCalculation(string bankName)
        {
            IBank bank = banks.FirstModel(bankName);

            double sumIncomes = bank.Clients.Sum(c => c.Income);
            double sumLoans = bank.Loans.Sum(a => a.Amount);
            double funds = sumIncomes + sumLoans;

            return $"The funds of bank {bankName} are {funds:F2}.";
        }

        public string ReturnLoan(string bankName, string loanTypeName)
        {
            IBank bank = banks.FirstModel(bankName);
            ILoan loan = loans.FirstModel(loanTypeName);

            if (loan == null)
            {
                throw new ArgumentException($"Loan of type {loanTypeName} is missing.");
            }

            bank.AddLoan(loan);
            loans.RemoveModel(loan);
            return $"{loanTypeName} successfully added to {bankName}.";
        }

        public string Statistics()
        {
            StringBuilder builder = new();

            foreach (IBank bank in banks.Models)
            {
                builder.AppendLine(bank.GetStatistics());
            }

            return builder.ToString().TrimEnd();
        }
    }
}
