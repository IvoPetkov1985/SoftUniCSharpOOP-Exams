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
        private Vehicle auto1;
        private Vehicle auto2;
        private Vehicle truck;
        private Vehicle microbus;

        [SetUp]
        public void Setup()
        {
            shop = new DealerShop(5);
            auto1 = new("VW", "Golf", 1998);
            auto2 = new("Audi", "Q7", 2022);
            truck = new("Iveco", "Model 2", 2020);
            microbus = new("DAF", "Sprinter", 2017);
        }

        [TestCase(1)]
        [TestCase(14)]
        [TestCase(30)]
        public void ConstructorShouldInitializeCorrectly(int capacity)
        {
            shop = new(capacity);
            Assert.IsNotNull(shop);
            Assert.That(capacity, Is.EqualTo(shop.Capacity));
            Assert.IsNotNull(shop.Vehicles);
        }

        [Test]
        public void CapacityShouldReturnTheCorrectValue()
        {
            int expectedValue = 5;
            int actualValue = shop.Capacity;
            Assert.That(expectedValue, Is.EqualTo(shop.Capacity));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-26)]
        public void CapacityShouldThrowIfNotPositive(int capacity)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => shop = new(capacity), "Capacity must be a positive value.");
        }

        [Test]
        public void VehiclesCountShouldBeZeroInitially()
        {
            int expectedValue = 0;
            int actualValue = shop.Vehicles.Count;
            Assert.That(expectedValue, Is.EqualTo(actualValue));
        }

        [Test]
        public void AddVehicleShouldReturnToStringOfTheAddedVehicle()
        {
            string firstMsg = $"Added {auto1.ToString()}";
            string secondMsg = $"Added {auto2.ToString()}";
            string firstActualMsg = shop.AddVehicle(auto1);
            string secondActualMsg = shop.AddVehicle(auto2);
            Assert.That(firstMsg, Is.EqualTo(firstActualMsg));
            Assert.That(secondMsg, Is.EqualTo(secondActualMsg));
            int expectedCount = 2;
            Assert.That(expectedCount, Is.EqualTo(shop.Vehicles.Count));
        }

        [Test]
        public void AddVehicleShouldThrowIfCapacityIsFull()
        {
            shop.AddVehicle(auto1);
            shop.AddVehicle(auto2);
            shop.AddVehicle(microbus);
            shop.AddVehicle(truck);
            Vehicle auto3 = new("Tesla", "Model 5", 2024);
            Vehicle auto4 = new("Mercedes", "S500", 2007);
            shop.AddVehicle(auto3);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.AddVehicle(auto4), "Inventory is full");
            int expectedCount = 5;
            int actualCount = shop.Vehicles.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
            Assert.IsFalse(shop.Vehicles.Contains(auto4));
        }

        [Test]
        public void VehiclesShouldContainAddedVehicles()
        {
            shop.AddVehicle(truck);
            shop.AddVehicle(microbus);
            Assert.IsTrue(shop.Vehicles.Contains(truck));
            Assert.IsTrue(shop.Vehicles.Contains(microbus));
            Assert.IsFalse(shop.Vehicles.Contains(auto2));
        }

        [Test]
        public void SellVehicleShouldReturnTrueIfSuccessful()
        {
            shop.AddVehicle(truck);
            shop.AddVehicle(microbus);
            shop.AddVehicle(auto1);
            Assert.IsTrue(shop.SellVehicle(truck));
            Assert.IsTrue(shop.SellVehicle(auto1));
            int expectedCount = 1;
            int actualCount = shop.Vehicles.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void SoldVehicleShouldRemoveItFromCollection()
        {
            shop.AddVehicle(truck);
            shop.AddVehicle(microbus);
            shop.AddVehicle(auto1);
            shop.SellVehicle(auto1);
            Assert.IsFalse(shop.Vehicles.Contains(auto1));
            int expectedCount = 2;
            Assert.That(expectedCount, Is.EqualTo(shop.Vehicles.Count));
        }

        [Test]
        public void SellVehicleShouldReturnFalseIfVehicleNotAdded()
        {
            shop.AddVehicle(truck);
            shop.AddVehicle(microbus);
            Assert.IsFalse(shop.SellVehicle(auto2));
        }

        [Test]
        public void InventoryReportShouldReturnTheCorrectString()
        {
            shop.AddVehicle(auto1);
            shop.AddVehicle(auto2);
            shop.AddVehicle(truck);

            StringBuilder builder = new();
            builder.AppendLine("Inventory Report");
            builder.AppendLine($"Capacity: {shop.Capacity}");
            builder.AppendLine($"Vehicles: {shop.Vehicles.Count}");
            builder.AppendLine(auto1.ToString());
            builder.AppendLine(auto2.ToString());
            builder.AppendLine(truck.ToString());

            string expectedResult = builder.ToString().TrimEnd();
            string actualResult = shop.InventoryReport();
            Assert.That(expectedResult, Is.EqualTo(actualResult));
        }
    }
}
