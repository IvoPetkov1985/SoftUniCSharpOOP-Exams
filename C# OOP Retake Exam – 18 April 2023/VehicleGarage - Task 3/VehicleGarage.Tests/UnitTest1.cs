using NUnit.Framework;
using System;
using System.Linq;

namespace VehicleGarage.Tests
{
    public class Tests
    {
        private Vehicle vehicle1;
        private Vehicle vehicle2;
        private Garage garage;

        [SetUp]
        public void Setup()
        {
            vehicle1 = new("VW", "Golf III Turbo", "PK1310BB");
            vehicle2 = new("Audi", "A3", "CA8899CC");
            garage = new(3);
            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
        }

        [Test]
        public void VehicleConstructorShouldInitializeCorrectly()
        {
            Assert.IsNotNull(vehicle1);
            Assert.IsNotNull(vehicle2);
        }

        [Test]
        public void VehicleConstructorShouldInitializeCorrectValues()
        {
            string expectedBrand = "VW";
            string expectedModel = "Golf III Turbo";
            string expectedPlateNumber = "PK1310BB";
            int expectedBatteryLevel = 100;

            Assert.That(expectedBrand == vehicle1.Brand);
            Assert.That(expectedModel == vehicle1.Model);
            Assert.That(expectedPlateNumber == vehicle1.LicensePlateNumber);
            Assert.That(expectedBatteryLevel == vehicle1.BatteryLevel);
            Assert.IsFalse(vehicle1.IsDamaged);
        }

        [Test]
        public void GarageConstructorShouldInitializeCorrectly()
        {
            Assert.IsNotNull(garage);
            Assert.IsNotNull(garage.Vehicles);
        }

        [TestCase(4)]
        [TestCase(10)]
        public void GarageShouldSetTheCorrectCapacity(int capacity)
        {
            Garage garage = new(capacity);
            Assert.That(capacity == garage.Capacity);
        }

        [Test]
        public void CapacityShouldReturnTheCorrectValue()
        {
            int expectedCapacity = 3;
            Assert.That(expectedCapacity == garage.Capacity);
        }

        [Test]
        public void AddVehicleShouldReturnTrueIfEverythingIsFine()
        {
            Vehicle vehicle3 = new("Audi", "Q7", "PB8989TT");
            Assert.IsTrue(garage.AddVehicle(vehicle3));
        }

        [Test]
        public void AddVehicleShouldReturnFalseIfCapacityExceeded()
        {
            Vehicle vehicle3 = new("Audi", "Q7", "PB8989TT");
            Vehicle vehicle4 = new("Tesla", "E4", "A3344KK");
            garage.AddVehicle(vehicle3);
            Assert.IsFalse(garage.AddVehicle(vehicle4));
        }

        [Test]
        public void AddVehicleShouldReturnFalseIfSamePlate()
        {
            Vehicle vehicle3 = new("Audi", "Q7", "CA8899CC");
            Assert.IsFalse(garage.AddVehicle(vehicle3));
        }

        [Test]
        public void AddVehicleShouldIncreaseTheCount()
        {
            Vehicle vehicle3 = new("Audi", "Q7", "PB8989TT");
            garage.AddVehicle(vehicle3);
            int expectedCount = 3;
            int actualCount = garage.Vehicles.Count;
            Assert.That(expectedCount == actualCount);
        }

        [Test]
        public void AddVehicleShouldReallyAddToTheCollection()
        {
            Vehicle vehicle3 = new("Audi", "Q7", "PB8989TT");
            garage.AddVehicle(vehicle3);
            Assert.IsNotNull(garage.Vehicles.FirstOrDefault(v => v.LicensePlateNumber == "PB8989TT"));
        }

        [Test]
        public void DriveVehicleShouldDecreaseBatteryLevel()
        {
            garage.DriveVehicle("CA8899CC", 50, false);
            garage.DriveVehicle("PK1310BB", 70, false);
            int expectedBatteryLevel = 50;
            int expectedSecondBattery = 30;
            Assert.That(expectedBatteryLevel.Equals(vehicle2.BatteryLevel));
            Assert.That(expectedSecondBattery.Equals(vehicle1.BatteryLevel));
        }

        [Test]
        public void DriveVehicleShouldChangeStatusIfAccidentHappens()
        {
            garage.DriveVehicle("CA8899CC", 50, true);
            Assert.IsTrue(vehicle2.IsDamaged);
        }

        [Test]
        public void DriveVehicleShouldNotChangeStatusIfNoAccidentHappens()
        {
            garage.DriveVehicle("CA8899CC", 44, false);
            garage.DriveVehicle("PK1310BB", 78, false);
            Assert.IsFalse(vehicle1.IsDamaged);
            Assert.IsFalse(vehicle2.IsDamaged);
        }

        [Test]
        public void DriveVehicleShouldStopIfTryingToDriveDamagedCar()
        {
            garage.DriveVehicle("CA8899CC", 50, true);
            garage.DriveVehicle("CA8899CC", 30, false);
            int expectedLevel = 50;
            Assert.That(expectedLevel == vehicle2.BatteryLevel);
        }

        [TestCase(101)]
        [TestCase(110)]
        public void DriveVehicleShouldStopIfInputDrainageAbove100(int drainage)
        {
            garage.DriveVehicle("PK1310BB", drainage, false);
            Assert.IsFalse(vehicle1.IsDamaged);
            int expectedLevel = 100;
            Assert.That(expectedLevel == vehicle1.BatteryLevel);
        }

        [Test]
        public void DriveVehicleShouldStopIfDrainageAboveTheBatteryLevel()
        {
            garage.DriveVehicle("PK1310BB", 70, false);
            garage.DriveVehicle("PK1310BB", 31, false);
            int expectedLevel = 30;
            Assert.That(expectedLevel == vehicle1.BatteryLevel);
        }

        [Test]
        public void DriveVehicleShouldNotChangeStatusIfNoAccident()
        {
            garage.DriveVehicle("PK1310BB", 60, false);
            Assert.IsFalse(vehicle1.IsDamaged);
        }

        [Test]
        public void ChargeVehiclesShouldReturnTheCorrectCount()
        {
            garage.DriveVehicle("PK1310BB", 90, false);
            garage.DriveVehicle("CA8899CC", 80, false);
            int expectedCount = 2;
            Assert.That(expectedCount == garage.ChargeVehicles(25));
        }

        [Test]
        public void ChargeVehiclesShouldChangeTheBatteryLevelTo100()
        {
            garage.DriveVehicle("PK1310BB", 95, false);
            garage.DriveVehicle("CA8899CC", 88, false);
            garage.ChargeVehicles(25);
            int expectedFirstCarLevel = 100;
            int expectedSecondCarLevel = 100;
            Assert.That(expectedFirstCarLevel == vehicle1.BatteryLevel);
            Assert.That(expectedSecondCarLevel == vehicle2.BatteryLevel);
        }

        [Test]
        public void ChargeVehiclesShouldNotChangeTheLevelIfEnoughPower()
        {
            garage.DriveVehicle("PK1310BB", 12, false);
            garage.DriveVehicle("CA8899CC", 45, false);
            garage.ChargeVehicles(50);
            int expectedFirstCarLevel = 88;
            int expectedSecondCarLevel = 55;
            int expectedChargedAutos = 0;
            Assert.That(expectedFirstCarLevel == vehicle1.BatteryLevel);
            Assert.That(expectedSecondCarLevel == vehicle2.BatteryLevel);
            Assert.That(expectedChargedAutos == garage.ChargeVehicles(50));
        }

        [Test]
        public void RepairVehiclesShouldReturnTheCorrectString()
        {
            Garage garage = new(5);
            garage.AddVehicle(new("Audi", "A4", "PB1313KK"));
            garage.AddVehicle(new("VW", "Passat", "CA3344HA"));
            garage.AddVehicle(new("VW", "Golf", "EH5566AA"));
            garage.AddVehicle(new("Ford", "Focus", "CB5543KK"));
            garage.AddVehicle(new("Seat", "Leon", "CO9098CC"));

            garage.DriveVehicle("PB1313KK", 70, true);
            garage.DriveVehicle("EH5566AA", 75, true);
            garage.DriveVehicle("CB5543KK", 45, true);
            garage.DriveVehicle("CO9098CC", 55, false);

            string expectedMessage = "Vehicles repaired: 3";
            string actualMessage = garage.RepairVehicles();

            Assert.That(expectedMessage == actualMessage);
        }

        [Test]
        public void RepairVehiclesShouldChangeStatus()
        {
            garage.DriveVehicle("PK1310BB", 70, true);
            garage.DriveVehicle("CA8899CC", 75, true);

            garage.RepairVehicles();
            Assert.That(vehicle1.IsDamaged == false);
            Assert.That(vehicle2.IsDamaged == false);
        }
    }
}
