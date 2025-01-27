using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace AutoTrade.Tests
{
    [TestFixture]
    public class DealerShopTests
    {
        private DealerShop shop;
        private Vehicle car;
        private Vehicle truck;
        private Vehicle bus;
        private Vehicle suv;

        [SetUp]
        public void Setup()
        {
            shop = new(4);
            car = new("Opel", "Vectra", 1999);
            truck = new("Volvo", "Aero", 2024);
            bus = new("Mercedes", "Citaro", 2011);
            suv = new("Audi", "Q7", 2017);
        }

        [TestCase(1)]
        [TestCase(13)]
        [TestCase(7)]
        public void ShopConstructorShouldInitializeCorrectly(int capacity)
        {
            DealerShop shop12 = new(capacity);
            Assert.IsNotNull(shop12);
            Assert.IsNotNull(shop12.Vehicles);
            Assert.That(capacity == shop12.Capacity);
        }

        [TestCase(0)]
        [TestCase(-2)]
        [TestCase(-10)]
        public void CapacityShouldThrowIfValueIsNegative(int capacity)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => shop = new(capacity), "Capacity must be a positive value.");
        }

        [Test]
        public void CapacityShouldBeSetCorrectly()
        {
            int expectedValue = 4;
            int actualValue = shop.Capacity;
            Assert.That(expectedValue == actualValue);
        }

        [Test]
        public void VehiclesCountShouldBeSetInitiallyToZero()
        {
            int expectedCount = 0;
            int actualCount = shop.Vehicles.Count;
            Assert.That(expectedCount == actualCount);
        }

        [Test]
        public void AddVehicleMethodShouldReturnTheCorrectString()
        {
            string expectedOutput = "Added 2024 Volvo Aero";
            string actualOutput = shop.AddVehicle(truck);
            Assert.That(expectedOutput == actualOutput);
        }

        [Test]
        public void AddVehicleMethodShouldIncrementTheCount()
        {
            shop.AddVehicle(car);
            shop.AddVehicle(truck);
            int expectedCount = 2;
            int actualCount = shop.Vehicles.Count;
            Assert.That(expectedCount == actualCount);
            Assert.IsTrue(shop.Vehicles.Contains(car));
            Assert.IsTrue(shop.Vehicles.Contains(truck));
        }

        [Test]
        public void AddVehicleMethodShouldThrowIfCapacityReached()
        {
            shop.AddVehicle(car);
            shop.AddVehicle(truck);
            shop.AddVehicle(bus);
            shop.AddVehicle(suv);
            Vehicle retroCar = new("Trabant", "500", 1970);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.AddVehicle(retroCar), "Inventory is full");
            Assert.IsFalse(shop.Vehicles.Contains(retroCar));
        }

        [Test]
        public void SellVehicleMethodShouldReturnTrueIfSuccessful()
        {
            shop.AddVehicle(car);
            shop.AddVehicle(truck);
            shop.AddVehicle(bus);
            shop.AddVehicle(suv);
            Assert.IsTrue(shop.SellVehicle(truck));
            Assert.IsFalse(shop.Vehicles.Contains(truck));
            int expectedCount = 3;
            int actualCount = shop.Vehicles.Count;
            Assert.That(expectedCount == actualCount);
        }

        [Test]
        public void SellVehicleMethodShoudReturnFalseIfNotSuccessful()
        {
            shop.AddVehicle(car);
            shop.AddVehicle(truck);
            Assert.IsFalse(shop.SellVehicle(bus));
            Assert.IsFalse(shop.Vehicles.Contains(bus));
        }

        [Test]
        public void ReportMethodShouldReturnTheCorrectText()
        {
            shop.AddVehicle(car);
            shop.AddVehicle(suv);
            shop.AddVehicle(truck);
            StringBuilder builder = new();
            builder.AppendLine("Inventory Report");
            builder.AppendLine("Capacity: 4");
            builder.AppendLine("Vehicles: 3");
            builder.AppendLine(car.ToString());
            builder.AppendLine(suv.ToString());
            builder.AppendLine(truck.ToString());
            string expectedResult = builder.ToString().TrimEnd();
            string actualResult = shop.InventoryReport();
            Assert.That(expectedResult == actualResult);
        }
    }
}
