using EDriveRent.Core.Contracts;
using EDriveRent.Models.Contracts;
using EDriveRent.Models.Routes;
using EDriveRent.Models.Users;
using EDriveRent.Models.Vehicles;
using EDriveRent.Repositories;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDriveRent.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IUser> users;
        private readonly IRepository<IVehicle> vehicles;
        private readonly IRepository<IRoute> routes;

        public Controller()
        {
            users = new UserRepository();
            vehicles = new VehicleRepository();
            routes = new RouteRepository();
        }

        public string AllowRoute(string startPoint, string endPoint, double length)
        {
            if (routes.GetAll().Any(r => r.StartPoint == startPoint && r.EndPoint == endPoint && r.Length == length))
            {
                return $"{startPoint}/{endPoint} - {length} km is already added in our platform.";
            }

            if (routes.GetAll().Any(r => r.StartPoint == startPoint && r.EndPoint == endPoint && r.Length < length))
            {
                return $"{startPoint}/{endPoint} shorter route is already added in our platform.";
            }

            int routeId = routes.GetAll().Count + 1;

            IRoute route = new Route(startPoint, endPoint, length, routeId);

            IRoute longerRoute = routes.GetAll().FirstOrDefault(r => r.StartPoint == startPoint && r.EndPoint == endPoint && r.Length > length);

            longerRoute?.LockRoute();

            routes.AddModel(route);
            return $"{startPoint}/{endPoint} - {length} km is unlocked in our platform.";
        }

        public string MakeTrip(string drivingLicenseNumber, string licensePlateNumber, string routeId, bool isAccidentHappened)
        {
            IUser user = users.FindById(drivingLicenseNumber);
            IVehicle vehicle = vehicles.FindById(licensePlateNumber);
            IRoute route = routes.FindById(routeId);

            if (user.IsBlocked)
            {
                return $"User {drivingLicenseNumber} is blocked in the platform! Trip is not allowed.";
            }

            if (vehicle.IsDamaged)
            {
                return $"Vehicle {licensePlateNumber} is damaged! Trip is not allowed.";
            }

            if (route.IsLocked)
            {
                return $"Route {routeId} is locked! Trip is not allowed.";
            }

            vehicle.Drive(route.Length);

            if (isAccidentHappened)
            {
                vehicle.ChangeStatus();
                user.DecreaseRating();
            }
            else
            {
                user.IncreaseRating();
            }

            return vehicle.ToString();
        }

        public string RegisterUser(string firstName, string lastName, string drivingLicenseNumber)
        {
            if (users.FindById(drivingLicenseNumber) != null)
            {
                return $"{drivingLicenseNumber} is already registered in our platform.";
            }

            IUser user = new User(firstName, lastName, drivingLicenseNumber);
            users.AddModel(user);
            return $"{firstName} {lastName} is registered successfully with DLN-{drivingLicenseNumber}";
        }

        public string RepairVehicles(int count)
        {
            IEnumerable<IVehicle> selectedVehicles = vehicles.GetAll()
                .Where(v => v.IsDamaged)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .Take(count);

            int countOfRepairedVehicles = selectedVehicles.Count();

            foreach (IVehicle vehicle in selectedVehicles)
            {
                vehicle.ChangeStatus();
                vehicle.Recharge();
            }

            return $"{countOfRepairedVehicles} vehicles are successfully repaired!";
        }

        public string UploadVehicle(string vehicleType, string brand, string model, string licensePlateNumber)
        {
            if (vehicleType != nameof(PassengerCar) &&
                vehicleType != nameof(CargoVan))
            {
                return $"{vehicleType} is not accessible in our platform.";
            }

            if (vehicles.FindById(licensePlateNumber) != null)
            {
                return $"{licensePlateNumber} belongs to another vehicle.";
            }

            IVehicle vehicle = null;

            if (vehicleType == nameof(PassengerCar))
            {
                vehicle = new PassengerCar(brand, model, licensePlateNumber);
            }
            else
            {
                vehicle = new CargoVan(brand, model, licensePlateNumber);
            }

            vehicles.AddModel(vehicle);
            return $"{brand} {model} is uploaded successfully with LPN-{licensePlateNumber}";
        }

        public string UsersReport()
        {
            StringBuilder builder = new();
            builder.AppendLine("*** E-Drive-Rent ***");

            foreach (IUser user in users.GetAll()
                .OrderByDescending(u => u.Rating)
                .ThenBy(u => u.LastName)
                .ThenBy(u => u.FirstName))
            {
                builder.AppendLine(user.ToString());
            }

            return builder.ToString().TrimEnd();
        }
    }
}
