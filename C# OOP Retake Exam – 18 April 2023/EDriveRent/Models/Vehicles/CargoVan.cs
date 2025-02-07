namespace EDriveRent.Models.Vehicles
{
    public class CargoVan : Vehicle
    {
        private const double CargoVanMaxMileage = 180;

        public CargoVan(string brand, string model, string licensePlateNumber)
            : base(brand, model, CargoVanMaxMileage, licensePlateNumber)
        {
        }
    }
}
