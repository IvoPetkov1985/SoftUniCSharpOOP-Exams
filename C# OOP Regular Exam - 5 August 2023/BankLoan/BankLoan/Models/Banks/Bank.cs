using BankLoan.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankLoan.Models.Banks
{
    public abstract class Bank : IBank
    {
        private string name;
        private readonly List<ILoan> loans;
        private readonly List<IClient> clients;

        protected Bank(string name, int capacity)
        {
            loans = new List<ILoan>();
            clients = new List<IClient>();
            Name = name;
            Capacity = capacity;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Bank name cannot be null or empty.");
                }

                name = value;
            }
        }

        public int Capacity { get; private set; }

        public IReadOnlyCollection<ILoan> Loans
            => loans.AsReadOnly();

        public IReadOnlyCollection<IClient> Clients
            => clients.AsReadOnly();

        public void AddClient(IClient client)
        {
            if (Clients.Count == Capacity)
            {
                throw new ArgumentException("Not enough capacity for this client.");
            }

            clients.Add(client);
        }

        public void AddLoan(ILoan loan)
        {
            loans.Add(loan);
        }

        public string GetStatistics()
        {
            IEnumerable<string> clientNames = Clients.Select(c => c.Name);
            StringBuilder builder = new();
            builder.AppendLine($"Name: {Name}, Type: {this.GetType().Name}");
            builder.AppendLine($"Clients: {(Clients.Any() ? string.Join(", ", clientNames) : "none")}");
            builder.AppendLine($"Loans: {Loans.Count}, Sum of Rates: {SumRates()}");
            return builder.ToString();
        }

        public void RemoveClient(IClient client)
        {
            clients.Remove(client);
        }

        public double SumRates()
        {
            return Loans.Sum(l => l.InterestRate);
        }
    }
}
