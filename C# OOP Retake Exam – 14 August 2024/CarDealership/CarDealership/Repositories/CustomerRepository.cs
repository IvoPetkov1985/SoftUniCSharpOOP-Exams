using CarDealership.Models.Contracts;
using CarDealership.Repositories.Contracts;

namespace CarDealership.Repositories
{
    public class CustomerRepository : IRepository<ICustomer>
    {
        private readonly List<ICustomer> customers;

        public CustomerRepository()
        {
            customers = new List<ICustomer>();
        }

        public IReadOnlyCollection<ICustomer> Models
            => customers.AsReadOnly();

        public void Add(ICustomer model)
        {
            customers.Add(model);
        }

        public bool Exists(string text)
        {
            return customers.Any(c => c.Name == text);
        }

        public ICustomer Get(string text)
        {
            return customers.FirstOrDefault(c => c.Name == text);
        }

        public bool Remove(string text)
        {
            ICustomer customer = customers.FirstOrDefault(c => c.Name == text);
            return customers.Remove(customer);
        }
    }
}
