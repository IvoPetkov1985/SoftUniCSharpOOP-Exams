using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace EDriveRent.Repositories
{
    public class UserRepository : IRepository<IUser>
    {
        private readonly List<IUser> users;

        public UserRepository()
        {
            this.users = new List<IUser>();
        }

        public void AddModel(IUser model)
        {
            users.Add(model);
        }

        public IUser FindById(string identifier)
        {
            IUser user = users.FirstOrDefault(u => u.DrivingLicenseNumber == identifier);
            return user;
        }

        public IReadOnlyCollection<IUser> GetAll()
        {
            return users.AsReadOnly();
        }

        public bool RemoveById(string identifier)
        {
            IUser user = FindById(identifier);
            return users.Remove(user);
        }
    }
}
