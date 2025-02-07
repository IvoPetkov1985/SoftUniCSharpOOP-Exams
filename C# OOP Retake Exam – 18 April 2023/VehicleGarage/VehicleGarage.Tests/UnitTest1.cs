using NUnit.Framework;
using System.Linq;

namespace VehicleGarage.Tests
{
    public class Tests
    {
        private Garage garage;
        private Vehicle truck;
        private Vehicle bus;
        private Vehicle car;
        private Vehicle suv;

        [SetUp]
        public void Setup()
        {
            garage = new(4);
            truck = new("Iveco", "Model11", "CB1914KK");
            bus = new("Mercedes", "Intouro", "CB2298HM");
            car = new("VW", "Golf II", "PK1919AB");
            suv = new("Audi", "Q7", "PB8888BB");
        }

        [Test]
        public void GarageConstructorShouldInitializeCorrectly()
        {
            Garage garage14 = new(14);
            Assert.IsNotNull(garage14);
            int expectedCapacity = 14;
            int actualCapacity = garage14.Capacity;
            Assert.That(expectedCapacity, Is.EqualTo(actualCapacity));
            Assert.IsNotNull(garage14.Vehicles);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(17)]
        public void GarageCapacityShouldSetTheCorrectValue(int capacity)
        {
            garage = new(capacity);
            Assert.That(capacity, Is.EqualTo(garage.Capacity));
        }

        [Test]
        public void VehiclesShouldHaveNoEntitiesInitially()
        {
            int expectedCount = 0;
            int actualCount = garage.Vehicles.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void AddVehicleShouldReturnTrueIfSuccessful()
        {
            Assert.IsTrue(garage.AddVehicle(truck));
            Assert.IsTrue(garage.AddVehicle(bus));
            Assert.IsTrue(garage.AddVehicle(suv));
            int expectedCount = 3;
            int actualCount = garage.Vehicles.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
            Assert.IsTrue(garage.Vehicles.Contains(truck));
        }

        [Test]
        public void AddVehicleShouldReturnFalseIfThePlateIsTheSame()
        {
            garage.AddVehicle(truck);
            garage.AddVehicle(car);
            Vehicle car2 = new("Mazda", "2", "CB1914KK");
            Assert.IsFalse(garage.AddVehicle(car2));
            Assert.IsFalse(garage.Vehicles.Contains(car2));
        }

        [Test]
        public void AddVehicleShouldReturnFalseIfCapacityExceeded()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.AddVehicle(truck);
            garage.AddVehicle(bus);
            Vehicle car2 = new("Mazda", "2", "CA1314CB");
            Assert.IsFalse(garage.AddVehicle(car2));
            Assert.IsFalse(garage.Vehicles.Contains(car2));
            int expectedCount = garage.Capacity;
            int actualCount = garage.Vehicles.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void ChargeVehiclesShouldReturnTheCorrectCount()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.AddVehicle(truck);
            garage.AddVehicle(bus);
            garage.DriveVehicle("CB1914KK", 70, false);
            garage.DriveVehicle("CB2298HM", 75, false);
            garage.DriveVehicle("PB8888BB", 20, false);
            int expectedResult = 2;
            int actualResult = garage.ChargeVehicles(30);
            Assert.That(expectedResult, Is.EqualTo(actualResult));
            int expectedLevel = 100;
            Assert.That(expectedLevel, Is.EqualTo(truck.BatteryLevel));
            Assert.That(expectedLevel, Is.EqualTo(bus.BatteryLevel));
        }

        [Test]
        public void ChargeVehicleShouldNotChargeIfBatteryNotLow()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.AddVehicle(truck);
            garage.AddVehicle(bus);
            garage.DriveVehicle("CB1914KK", 70, false);
            garage.DriveVehicle("CB2298HM", 74, false);
            garage.DriveVehicle("PB8888BB", 20, false);
            int expectedCount = 0;
            int actualCount = garage.ChargeVehicles(25);
            Assert.That(expectedCount, Is.EqualTo(actualCount));
            int expectedTruckBattery = 30;
            int actualTruckBattery = truck.BatteryLevel;
            Assert.That(expectedTruckBattery, Is.EqualTo(actualTruckBattery));
        }

        [Test]
        public void DriveVehicleShouldChangeTheStatusIfAccidentOccures()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.DriveVehicle("PK1919AB", 10, true);
            garage.DriveVehicle("PB8888BB", 15, false);
            Assert.IsTrue(car.IsDamaged);
            Assert.IsFalse(suv.IsDamaged);
        }

        [Test]
        public void DriveVehicleShouldDecreaseBatteryLevelCorrectly()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.DriveVehicle("PK1919AB", 10, false);
            garage.DriveVehicle("PB8888BB", 15, false);
            int expectedCarLevel = 90;
            int expectedSuvLevel = 85;
            Assert.That(expectedCarLevel, Is.EqualTo(car.BatteryLevel));
            Assert.That(expectedSuvLevel, Is.EqualTo(suv.BatteryLevel));
        }

        [Test]
        public void DriveVehicleShouldNotWorkIfBatteryTooLow()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.DriveVehicle("PK1919AB", 60, false);
            garage.DriveVehicle("PB8888BB", 75, false);
            garage.DriveVehicle("PK1919AB", 41, false);
            garage.DriveVehicle("PB8888BB", 26, false);
            int expectedCarLevel = 40;
            int expectedSuvLevel = 25;
            Assert.That(expectedCarLevel, Is.EqualTo(car.BatteryLevel));
            Assert.That(expectedSuvLevel, Is.EqualTo(suv.BatteryLevel));
        }

        [Test]
        public void DriveVehicleShouldNotWorkIfDrainageOver100()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.DriveVehicle("PK1919AB", 160, false);
            garage.DriveVehicle("PB8888BB", 101, false);
            int expectedLevel = 100;
            Assert.That(expectedLevel, Is.EqualTo(car.BatteryLevel));
            Assert.That(expectedLevel, Is.EqualTo(suv.BatteryLevel));
        }

        [Test]
        public void DriveVehicleShouldNotWorkIfCarIsDamaged()
        {
            garage.AddVehicle(suv);
            garage.DriveVehicle("PB8888BB", 50, true);
            garage.DriveVehicle("PB8888BB", 40, false);
            int expectedLevel = 50;
            int actualLevel = suv.BatteryLevel;
            Assert.That(expectedLevel, Is.EqualTo(actualLevel));
            Assert.IsTrue(garage.Vehicles.Any(v => v.IsDamaged));
        }

        [Test]
        public void RepairVehiclesShouldChangeTheStatus()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.AddVehicle(truck);
            garage.AddVehicle(bus);
            garage.DriveVehicle("PK1919AB", 45, true);
            garage.DriveVehicle("PB8888BB", 51, true);
            garage.DriveVehicle("CB2298HM", 38, false);
            garage.RepairVehicles();
            Assert.IsTrue(garage.Vehicles.All(v => v.IsDamaged == false));
            Assert.IsFalse(suv.IsDamaged);
            Assert.IsFalse(car.IsDamaged);
        }

        [Test]
        public void RepairVehiclesShouldReturnTheCorrectString()
        {
            garage.AddVehicle(car);
            garage.AddVehicle(suv);
            garage.AddVehicle(truck);
            garage.AddVehicle(bus);
            garage.DriveVehicle("PK1919AB", 45, false);
            garage.DriveVehicle("PB8888BB", 51, true);
            garage.DriveVehicle("CB2298HM", 38, true);
            string expectedString = "Vehicles repaired: 2";
            string actualString = garage.RepairVehicles();
            Assert.That(expectedString, Is.EqualTo(actualString));
        }
    }
}
