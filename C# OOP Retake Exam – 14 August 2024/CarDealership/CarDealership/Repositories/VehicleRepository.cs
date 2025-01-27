using CarDealership.Models.Contracts;
using CarDealership.Repositories.Contracts;

namespace CarDealership.Repositories
{
    public class VehicleRepository : IRepository<IVehicle>
    {
        private readonly List<IVehicle> vehicles;

        public VehicleRepository()
        {
            vehicles = new List<IVehicle>();
        }

        public IReadOnlyCollection<IVehicle> Models
            => vehicles.AsReadOnly();

        public void Add(IVehicle model)
        {
            vehicles.Add(model);
        }

        public bool Exists(string text)
        {
            return vehicles.Any(v => v.Model == text);
        }

        public IVehicle Get(string text)
        {
            return vehicles.FirstOrDefault(v => v.Model == text);
        }

        public bool Remove(string text)
        {
            IVehicle vehicle = vehicles.FirstOrDefault(v => v.Model == text);
            return vehicles.Remove(vehicle);
        }
    }
}
