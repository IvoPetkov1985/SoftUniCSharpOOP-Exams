using CarDealership.Core.Contracts;
using CarDealership.Models;
using CarDealership.Models.Contracts;
using System.Text;

namespace CarDealership.Core
{
    public class Controller : IController
    {
        private readonly IDealership dealership;

        public Controller()
        {
            dealership = new Dealership();
        }

        public string AddCustomer(string customerTypeName, string customerName)
        {
            if (customerTypeName != nameof(IndividualClient) &&
                customerTypeName != nameof(LegalEntityCustomer))
            {
                return $"{customerTypeName} is not a valid type.";
            }

            ICustomer customer = dealership.Customers.Get(customerName);

            if (customer != null)
            {
                return $"{customerName} already exists as a profile in the dealership.";
            }

            if (customerTypeName == nameof(IndividualClient))
            {
                customer = new IndividualClient(customerName);
            }
            else
            {
                customer = new LegalEntityCustomer(customerName);
            }

            dealership.Customers.Add(customer);
            return $"{customerName} created a profile in the dealership.";
        }

        public string AddVehicle(string vehicleTypeName, string model, double price)
        {
            if (vehicleTypeName != nameof(SaloonCar) &&
                vehicleTypeName != nameof(SUV) &&
                vehicleTypeName != nameof(Truck))
            {
                return $"{vehicleTypeName} is not a valid type.";
            }

            IVehicle vehicle = dealership.Vehicles.Get(model);

            if (vehicle != null)
            {
                return $"{model} already exists as an offer in the dealership.";
            }

            if (vehicleTypeName == nameof(SaloonCar))
            {
                vehicle = new SaloonCar(model, price);
            }
            else if (vehicleTypeName == nameof(SUV))
            {
                vehicle = new SUV(model, price);
            }
            else
            {
                vehicle = new Truck(model, price);
            }

            dealership.Vehicles.Add(vehicle);
            return $"{vehicleTypeName}: {model} is listed in the dealership. Price: {vehicle.Price:F2}";
        }

        public string CustomerReport()
        {
            StringBuilder builder = new();
            builder.AppendLine("Customer Report:");

            foreach (ICustomer customer in dealership.Customers.Models
                .OrderBy(c => c.Name))
            {
                builder.AppendLine(customer.ToString());
                builder.AppendLine("-Models:");

                if (customer.Purchases.Any())
                {
                    foreach (string model in customer.Purchases
                        .OrderBy(x => x))
                    {
                        IVehicle vehicle = dealership.Vehicles.Get(model);
                        builder.AppendLine($"--{vehicle.Model}");
                    }
                }
                else
                {
                    builder.AppendLine("--none");
                }
            }

            return builder.ToString().TrimEnd();
        }

        public string PurchaseVehicle(string vehicleTypeName, string customerName, double budget)
        {
            ICustomer customer = dealership.Customers.Get(customerName);

            if (customer == null)
            {
                return $"{customerName} has no profile in the dealership.";
            }

            if (!dealership.Vehicles.Models.Any(v => v.GetType().Name == vehicleTypeName))
            {
                return $"{vehicleTypeName} is not listed for sale in the dealership.";
            }

            if ((customer.GetType().Name == nameof(IndividualClient)
                && vehicleTypeName != nameof(SaloonCar)
                && vehicleTypeName != nameof(SUV)) ||
                (customer.GetType().Name == nameof(LegalEntityCustomer)
                && vehicleTypeName != nameof(SUV)
                && vehicleTypeName != nameof(Truck)))
            {
                return $"{customerName} is not eligible to purchase a {vehicleTypeName}.";
            }

            IEnumerable<IVehicle> availableVehicles = dealership.Vehicles.Models
                .Where(v => v.GetType().Name == vehicleTypeName && v.Price <= budget);

            if (!availableVehicles.Any())
            {
                return $"{customerName} does not have enough budget to purchase {vehicleTypeName}.";
            }

            IVehicle vehicleToSell = availableVehicles
                .OrderByDescending(v => v.Price)
                .First();

            customer.BuyVehicle(vehicleToSell.Model);
            vehicleToSell.SellVehicle(customerName);
            return $"{customerName} purchased a {vehicleToSell.Model}.";
        }

        public string SalesReport(string vehicleTypeName)
        {
            IEnumerable<IVehicle> selectedVehicles = dealership.Vehicles.Models
                .Where(v => v.GetType().Name == vehicleTypeName)
                .OrderBy(v => v.Model);

            StringBuilder builder = new();
            builder.AppendLine($"{vehicleTypeName} Sales Report:");

            foreach (IVehicle vehicle in selectedVehicles)
            {
                builder.AppendLine($"--{vehicle.ToString()}");
            }

            int sumPurchases = selectedVehicles.Sum(v => v.SalesCount);
            builder.AppendLine($"-Total Purchases: {sumPurchases}");

            return builder.ToString().TrimEnd();
        }
    }
}
