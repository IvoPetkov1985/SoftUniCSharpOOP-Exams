namespace CarDealership.Models.Vehicles
{
    public class Truck : Vehicle
    {
        private const double TruckPriceModifier = 1.30;

        public Truck(string model, double price)
            : base(model, price * TruckPriceModifier)
        {
        }
    }
}
