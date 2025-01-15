using BlackFriday.Models.Contracts;
using BlackFriday.Repositories.Contracts;

namespace BlackFriday.Repositories
{
    public class ProductRepository : IRepository<IProduct>
    {
        private readonly List<IProduct> products;

        public ProductRepository()
        {
            products = new List<IProduct>();
        }

        public IReadOnlyCollection<IProduct> Models
            => products.AsReadOnly();

        public void AddNew(IProduct model)
        {
            products.Add(model);
        }

        public bool Exists(string name)
        {
            return products.Any(p => p.ProductName == name);
        }

        public IProduct GetByName(string name)
        {
            IProduct product = products.FirstOrDefault(p => p.ProductName == name);
            return product;
        }
    }
}
