using CarDealership.Models.Contracts;

namespace CarDealership.Models.Customers
{
    public abstract class Customer : ICustomer
    {
        private string name;
        private readonly List<string> purchases;

        protected Customer(string name)
        {
            Name = name;
            purchases = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name is required.");
                }

                name = value;
            }
        }

        public IReadOnlyCollection<string> Purchases
            => purchases.AsReadOnly();

        public void BuyVehicle(string vehicleModel)
        {
            purchases.Add(vehicleModel);
        }

        public override string ToString()
        {
            return $"{Name} - Purchases: {Purchases.Count}";
        }
    }
}
