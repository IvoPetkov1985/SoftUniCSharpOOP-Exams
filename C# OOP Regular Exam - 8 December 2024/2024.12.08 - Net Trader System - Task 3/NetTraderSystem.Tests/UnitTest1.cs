using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetTraderSystem.Tests
{
    public class Tests
    {
        private TradingPlatform platform;
        private Product product1;
        private Product product2;
        private Product product3;

        [SetUp]
        public void Setup()
        {
            platform = new TradingPlatform(4);
            product1 = new("Windows", "OS", 180.00);
            product2 = new("Eset AV", "AntiVirus", 75.25);
            product3 = new("OfficeSuite", "Software", 133.50);
        }

        [TestCase(5)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(15)]
        public void ConstructorShouldInitializeCorrectData(int limit)
        {
            platform = new(limit);
            Assert.IsNotNull(platform);
            Assert.IsNotNull(platform.Products);
            Assert.IsTrue(platform.Products.Count == 0);
        }

        [Test]
        public void ProductsShouldHaveZeroCountUponInitialization()
        {
            Assert.IsEmpty(platform.Products);
        }

        [Test]
        public void AddingProductShouldReturnTheCorrectMessage()
        {
            string expectedFirstMsg = "Product Windows added successfully";
            string expectedSecondMsg = "Product Eset AV added successfully";
            string actualFirstMsg = platform.AddProduct(product1);
            string actualSecondMsg = platform.AddProduct(product2);
            int expectedCount = 2;
            Assert.That(expectedFirstMsg, Is.EqualTo(actualFirstMsg));
            Assert.That(expectedSecondMsg, Is.EqualTo(actualSecondMsg));
            Assert.That(expectedCount, Is.EqualTo(platform.Products.Count));
            Assert.IsTrue(platform.Products.Contains(product2));
        }

        [Test]
        public void AddingProductShouldReturnTheCorrectMessageIfInventoryIsFull()
        {
            platform.AddProduct(product1);
            platform.AddProduct(product2);
            platform.AddProduct(product3);
            Product product4 = new("WinRar", "Archivator", 35.99);
            Product product5 = new("AdBlocker", "AntiSpyware", 78.80);
            platform.AddProduct(product4);
            string expectedMsg = "Inventory is full";
            int expectedCount = 4;
            string actualMsg = platform.AddProduct(product5);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            Assert.That(expectedCount, Is.EqualTo(platform.Products.Count));
        }

        [Test]
        public void RemoveProductShouldReturnTrueIfExisting()
        {
            platform.AddProduct(product1);
            platform.AddProduct(product2);
            platform.AddProduct(product3);
            Assert.IsTrue(platform.RemoveProduct(product1));
            Assert.IsTrue(platform.RemoveProduct(product3));
            int expectedCount = 1;
            Assert.That(expectedCount, Is.EqualTo(platform.Products.Count));
            Assert.IsFalse(platform.Products.Contains(product1));
            Assert.IsFalse(platform.Products.Contains(product3));
        }

        [Test]
        public void RemoveProductShouldReturnFalseIfProductNotAdded()
        {
            platform.AddProduct(product2);
            platform.AddProduct(product3);
            Assert.IsFalse(platform.RemoveProduct(product1));
        }

        [Test]
        public void SellProductShouldReturnTheSoldProduct()
        {
            platform.AddProduct(product1);
            platform.AddProduct(product2);
            platform.AddProduct(product3);
            Assert.That(product3, Is.EqualTo(platform.SellProduct(product3)));
            Assert.That(product1, Is.EqualTo(platform.SellProduct(product1)));
            int expectedCount = 1;
            Assert.That(expectedCount, Is.EqualTo(platform.Products.Count));
            Assert.IsFalse(platform.Products.Contains(product3));
        }

        [Test]
        public void SellProductShouldReturnNullIfProductNotOnThePlatform()
        {
            platform.AddProduct(product1);
            platform.AddProduct(product3);
            Assert.IsNull(platform.SellProduct(product2));
            Product product4 = new("VSPlugin2024", "Extension for VS", 13.45);
            Assert.IsNull(platform.SellProduct(product4));
        }

        [Test]
        public void InventoryReportShouldReturnTheCorrectString()
        {
            platform.AddProduct(product1);
            platform.AddProduct(product2);
            platform.AddProduct(product3);

            StringBuilder builder = new();
            builder.AppendLine("Inventory Report:");
            builder.AppendLine("Available Products: 3");
            builder.AppendLine(product1.ToString());
            builder.AppendLine(product2.ToString());
            builder.AppendLine(product3.ToString());
            string expectedStr = builder.ToString().TrimEnd();
            string actualStr = platform.InventoryReport();

            Assert.That(expectedStr, Is.EqualTo(actualStr));
        }
    }
}
