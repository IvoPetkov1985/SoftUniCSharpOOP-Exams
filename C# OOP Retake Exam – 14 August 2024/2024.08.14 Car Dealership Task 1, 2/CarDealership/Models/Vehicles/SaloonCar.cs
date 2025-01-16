namespace CarDealership.Models.Vehicles
{
    public class SaloonCar : Vehicle
    {
        private const double SaloonCarPriceModifier = 1.10;

        public SaloonCar(string model, double price)
            : base(model, price * SaloonCarPriceModifier)
        {
        }
    }
}
