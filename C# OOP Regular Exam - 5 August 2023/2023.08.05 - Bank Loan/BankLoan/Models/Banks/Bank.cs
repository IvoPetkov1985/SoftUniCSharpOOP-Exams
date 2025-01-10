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
            Name = name;
            Capacity = capacity;

            loans = new List<ILoan>();
            clients = new List<IClient>();
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

        public IReadOnlyCollection<ILoan> Loans => loans;

        public IReadOnlyCollection<IClient> Clients => clients;

        public void AddClient(IClient Client)
        {
            if (clients.Count < Capacity)
            {
                clients.Add(Client);
            }
            else
            {
                throw new ArgumentException("Not enough capacity for this client.");
            }
        }

        public void AddLoan(ILoan loan)
        {
            loans.Add(loan);
        }

        public string GetStatistics()
        {
            StringBuilder builder = new();

            builder.AppendLine($"Name: {Name}, Type: {this.GetType().Name}");
            builder.AppendLine($"Clients: {(clients.Any() ? string.Join(", ", clients.Select(c => c.Name)) : "none")}");
            builder.AppendLine($"Loans: {loans.Count}, Sum of Rates: {SumRates()}");

            return builder.ToString().TrimEnd();
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
