using CarDealership.Models.Contracts;

namespace CarDealership.Models.Vehicles
{
    public abstract class Vehicle : IVehicle
    {
        private string model;
        private double price;
        private readonly List<string> buyers;

        protected Vehicle(string model, double price)
        {
            Model = model;
            Price = price;
            buyers = new List<string>();
        }

        public string Model
        {
            get => model;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Model is required.");
                }

                model = value;
            }
        }

        public double Price
        {
            get => price;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Price must be a positive number.");
                }

                price = value;
            }
        }

        public IReadOnlyCollection<string> Buyers
            => buyers.AsReadOnly();

        public int SalesCount
            => Buyers.Count;

        public void SellVehicle(string buyerName)
        {
            buyers.Add(buyerName);
        }

        public override string ToString()
        {
            return $"{Model} - Price: {Price:F2}, Total Model Sales: {SalesCount}";
        }
    }
}
