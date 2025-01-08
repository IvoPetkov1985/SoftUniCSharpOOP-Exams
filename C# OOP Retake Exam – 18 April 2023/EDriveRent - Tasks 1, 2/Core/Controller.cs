using EDriveRent.Core.Contracts;
using EDriveRent.Models;
using EDriveRent.Models.Contracts;
using EDriveRent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDriveRent.Core
{
    public class Controller : IController
    {
        private readonly UserRepository users;
        private readonly VehicleRepository vehicles;
        private readonly RouteRepository routes;

        public Controller()
        {
            users = new UserRepository();
            vehicles = new VehicleRepository();
            routes = new RouteRepository();
        }

        public string AllowRoute(string startPoint, string endPoint, double length)
        {
            IRoute route = routes.GetAll().FirstOrDefault(r => r.StartPoint == startPoint && r.EndPoint == endPoint && r.Length == length);

            if (route != null)
            {
                return $"{startPoint}/{endPoint} - {length} km is already added in our platform.";
            }

            IRoute shorterRoute = routes.GetAll().FirstOrDefault(r => r.StartPoint == startPoint && r.EndPoint == endPoint && r.Length < length);

            if (shorterRoute != null)
            {
                return $"{startPoint}/{endPoint} shorter route is already added in our platform.";
            }

            int routeId = routes.GetAll().Count + 1;
            IRoute newRoute = new Route(startPoint, endPoint, length, routeId);

            IRoute longerRoute = routes.GetAll().FirstOrDefault(r => r.StartPoint == startPoint && r.EndPoint == endPoint && r.Length > length);

            if (longerRoute != null)
            {
                longerRoute.LockRoute();
            }

            routes.AddModel(newRoute);
            return $"{startPoint}/{endPoint} - {length} km is unlocked in our platform.";
        }

        public string MakeTrip(string drivingLicenseNumber, string licensePlateNumber, string routeId, bool isAccidentHappened)
        {
            IUser user = users.FindById(drivingLicenseNumber);
            IRoute route = routes.FindById(routeId);
            IVehicle vehicle = vehicles.FindById(licensePlateNumber);

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
            IUser user = users.FindById(drivingLicenseNumber);

            if (user != null)
            {
                return $"{drivingLicenseNumber} is already registered in our platform.";
            }

            user = new User(firstName, lastName, drivingLicenseNumber);
            users.AddModel(user);

            return $"{firstName} {lastName} is registered successfully with DLN-{drivingLicenseNumber}";
        }

        public string RepairVehicles(int count)
        {
            List<IVehicle> vehiclesToRepair = vehicles.GetAll()
                .Where(v => v.IsDamaged)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .Take(count)
                .ToList();

            foreach (IVehicle vehicle in vehiclesToRepair)
            {
                vehicle.ChangeStatus();
                vehicle.Recharge();
            }

            return $"{vehiclesToRepair.Count} vehicles are successfully repaired!";
        }

        public string UploadVehicle(string vehicleType, string brand, string model, string licensePlateNumber)
        {
            if (vehicleType != nameof(CargoVan) && vehicleType != nameof(PassengerCar))
            {
                return $"{vehicleType} is not accessible in our platform.";
            }

            IVehicle vehicle = vehicles.FindById(licensePlateNumber);

            if (vehicle != null)
            {
                return $"{licensePlateNumber} belongs to another vehicle.";
            }

            switch (vehicleType)
            {
                case "CargoVan":
                    vehicle = new CargoVan(brand, model, licensePlateNumber);
                    break;
                case "PassengerCar":
                    vehicle = new PassengerCar(brand, model, licensePlateNumber);
                    break;
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
