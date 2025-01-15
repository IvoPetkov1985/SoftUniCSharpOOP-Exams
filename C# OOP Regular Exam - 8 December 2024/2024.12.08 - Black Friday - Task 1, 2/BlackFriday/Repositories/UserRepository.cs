using BlackFriday.Models.Contracts;
using BlackFriday.Repositories.Contracts;

namespace BlackFriday.Repositories
{
    public class UserRepository : IRepository<IUser>
    {
        private readonly List<IUser> users;

        public UserRepository()
        {
            users = new List<IUser>();
        }

        public IReadOnlyCollection<IUser> Models
            => users.AsReadOnly();

        public void AddNew(IUser model)
        {
            users.Add(model);
        }

        public bool Exists(string name)
        {
            return users.Any(u => u.UserName == name);
        }

        public IUser GetByName(string name)
        {
            IUser user = users.FirstOrDefault(u => u.UserName == name);
            return user;
        }
    }
}
