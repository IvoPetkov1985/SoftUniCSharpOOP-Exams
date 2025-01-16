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

        public void Add(ICustomer customer)
        {
            customers.Add(customer);
        }

        public bool Exists(string name)
        {
            return customers.Any(c => c.Name == name);
        }

        public ICustomer Get(string name)
        {
            return customers.FirstOrDefault(c => c.Name == name);
        }

        public bool Remove(string name)
        {
            return customers.Remove(Get(name));
        }
    }
}
