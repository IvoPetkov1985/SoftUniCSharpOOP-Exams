using BlackFriday.Models.Contracts;
using BlackFriday.Repositories;
using BlackFriday.Repositories.Contracts;

namespace BlackFriday.Models.Apps
{
    public class Application : IApplication
    {
        private readonly IRepository<IProduct> products;
        private readonly IRepository<IUser> users;

        public Application()
        {
            products = new ProductRepository();
            users = new UserRepository();
        }

        public IRepository<IProduct> Products
            => products;

        public IRepository<IUser> Users
            => users;
    }
}
