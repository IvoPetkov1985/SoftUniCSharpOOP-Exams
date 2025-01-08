using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace EDriveRent.Repositories
{
    public class VehicleRepository : IRepository<IVehicle>
    {
        private readonly List<IVehicle> vehicles;

        public VehicleRepository()
        {
            this.vehicles = new List<IVehicle>();
        }

        public void AddModel(IVehicle model)
        {
            vehicles.Add(model);
        }

        public IVehicle FindById(string identifier)
        {
            IVehicle vehicle = vehicles.FirstOrDefault(v => v.LicensePlateNumber == identifier);
            return vehicle;
        }

        public IReadOnlyCollection<IVehicle> GetAll()
        {
            return vehicles.AsReadOnly();
        }

        public bool RemoveById(string identifier)
        {
            IVehicle vehicle = FindById(identifier);
            return vehicles.Remove(vehicle);
        }
    }
}
