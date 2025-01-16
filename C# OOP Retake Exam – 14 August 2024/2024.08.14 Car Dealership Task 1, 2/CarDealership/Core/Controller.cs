using CarDealership.Core.Contracts;
using CarDealership.Models.Apps;
using CarDealership.Models.Contracts;
using CarDealership.Models.Customers;
using CarDealership.Models.Vehicles;
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

            if (dealership.Customers.Exists(customerName))
            {
                return $"{customerName} already exists as a profile in the dealership.";
            }

            ICustomer customer = null;

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

            if (dealership.Vehicles.Exists(model))
            {
                return $"{model} already exists as an offer in the dealership.";
            }

            IVehicle vehicle = null;

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

                if (customer.Purchases.Count == 0)
                {
                    builder.AppendLine("--none");
                }
                else
                {
                    foreach (string model in customer.Purchases
                        .OrderBy(x => x))
                    {
                        builder.AppendLine($"--{model}");
                    }
                }
            }

            return builder.ToString().TrimEnd();
        }

        public string PurchaseVehicle(string vehicleTypeName, string customerName, double budget)
        {
            if (dealership.Customers.Exists(customerName) == false)
            {
                return $"{customerName} has no profile in the dealership.";
            }

            if (vehicleTypeName != nameof(SaloonCar) &&
                vehicleTypeName != nameof(SUV) &&
                vehicleTypeName != nameof(Truck))
            {
                return $"{vehicleTypeName} is not listed for sale in the dealership.";
            }

            ICustomer customer = dealership.Customers.Get(customerName);

            if ((customer.GetType().Name == nameof(IndividualClient) && vehicleTypeName == nameof(Truck)) ||
                (customer.GetType().Name == nameof(LegalEntityCustomer) && vehicleTypeName == nameof(SaloonCar)))
            {
                return $"{customerName} is not eligible to purchase a {vehicleTypeName}.";
            }

            IEnumerable<IVehicle> selectedVehicles = dealership.Vehicles.Models
                .Where(v => v.GetType().Name == vehicleTypeName && v.Price <= budget)
                .OrderByDescending(v => v.Price);

            if (selectedVehicles.Any() == false)
            {
                return $"{customerName} does not have enough budget to purchase {vehicleTypeName}.";
            }
            else
            {
                IVehicle vehicleToSell = selectedVehicles.First();
                customer.BuyVehicle(vehicleToSell.Model);
                vehicleToSell.SellVehicle(customerName);
                return $"{customerName} purchased a {vehicleToSell.Model}.";
            }
        }

        public string SalesReport(string vehicleTypeName)
        {
            StringBuilder builder = new();
            builder.AppendLine($"{vehicleTypeName} Sales Report:");

            IEnumerable<IVehicle> selected = dealership.Vehicles.Models
                .Where(v => v.GetType().Name == vehicleTypeName)
                .OrderBy(v => v.Model);

            foreach (IVehicle vehicle in selected)
            {
                builder.AppendLine($"--{vehicle.ToString()}");
            }

            builder.AppendLine($"-Total Purchases: {selected.Sum(v => v.SalesCount)}");

            return builder.ToString().TrimEnd();
        }
    }
}
