using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace EDriveRent.Repositories
{
    public class RouteRepository : IRepository<IRoute>
    {
        private readonly List<IRoute> routes;

        public RouteRepository()
        {
            this.routes = new List<IRoute>();
        }

        public void AddModel(IRoute model)
        {
            routes.Add(model);
        }

        public IRoute FindById(string identifier)
        {
            int id = int.Parse(identifier);

            IRoute route = routes.FirstOrDefault(r => r.RouteId == id);
            return route;
        }

        public IReadOnlyCollection<IRoute> GetAll()
        {
            return routes.AsReadOnly();
        }

        public bool RemoveById(string identifier)
        {
            IRoute route = FindById(identifier);
            return routes.Remove(route);
        }
    }
}
