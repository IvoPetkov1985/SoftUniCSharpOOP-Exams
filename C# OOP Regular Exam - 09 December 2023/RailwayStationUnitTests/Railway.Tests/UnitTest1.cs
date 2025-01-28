namespace Railway.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;

    public class Tests
    {
        private RailwayStation station;

        [SetUp]
        public void Setup()
        {
            station = new("Gorna Banya");
        }

        [TestCase("Stara Zagora")]
        [TestCase("Kazanlak")]
        [TestCase("Tulovo")]
        public void ConstructorShouldInitializeNameCorrectly(string name)
        {
            RailwayStation railway = new(name);
            Assert.That(name == railway.Name);
        }

        [Test]
        public void ConstructorShouldInitializeCollectionsCorrectly()
        {
            RailwayStation railway = new("Poduene");
            Assert.IsNotNull(railway);
            Assert.IsNotNull(railway.ArrivalTrains);
            Assert.IsNotNull(railway.DepartureTrains);
        }

        [Test]
        public void RaiwayNameShouldBeCorrect()
        {
            string expectedName = "Gorna Banya";
            string actualName = station.Name;
            Assert.That(expectedName == actualName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void RailwayNameShouldThrowIfNullOrEmpty(string name)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => station = new(name), "Name cannot be null or empty!");
        }

        [Test]
        public void NewArrivalShouldEnqueueTheCorrectInfo()
        {
            station.NewArrivalOnBoard("Sofia -> Burgas");
            station.NewArrivalOnBoard("Sofia -> Gorna Oryahovica");
            int expectedCount = 2;
            int actualCount = station.ArrivalTrains.Count;
            Assert.That(expectedCount == actualCount);
            Assert.That(station.ArrivalTrains.Contains("Sofia -> Burgas"));
        }

        [Test]
        public void TrainHasArrivedShouldReturnTheCorrectString()
        {
            station.NewArrivalOnBoard("Sofia -> Burgas");
            station.NewArrivalOnBoard("Sofia -> Gorna Oryahovica");
            string expectedMsg = "Sofia -> Burgas is on the platform and will leave in 5 minutes.";
            string actualMsg = station.TrainHasArrived("Sofia -> Burgas");
            Assert.That(expectedMsg == actualMsg);
            int expectedDepTrCount = 1;
            int expectedArrTrCount = 1;
            Assert.That(expectedDepTrCount == station.DepartureTrains.Count);
            Assert.That(expectedArrTrCount == station.ArrivalTrains.Count);
        }

        [Test]
        public void TrainHasArrivedShouldMakeTheCorrectOperations()
        {
            station.NewArrivalOnBoard("Sofia -> Burgas");
            station.NewArrivalOnBoard("Sofia -> Gorna Oryahovica");
            station.NewArrivalOnBoard("Dragoman -> Plovdiv");
            station.TrainHasArrived("Sofia -> Burgas");
            station.TrainHasArrived("Sofia -> Gorna Oryahovica");
            Assert.IsTrue(station.DepartureTrains.Contains("Sofia -> Burgas"));
            Assert.IsFalse(station.ArrivalTrains.Contains("Sofia -> Burgas"));
        }

        [Test]
        public void TrainHasArrivedShouldReturnTheCorrectMessageIfQueueRowIncorrect()
        {
            station.NewArrivalOnBoard("Sofia -> Burgas");
            station.NewArrivalOnBoard("Sofia -> Gorna Oryahovica");
            station.NewArrivalOnBoard("Dragoman -> Plovdiv");
            string expectedMsg = "There are other trains to arrive before Dragoman -> Plovdiv.";
            string actualMsg = station.TrainHasArrived("Dragoman -> Plovdiv");
            Assert.That(expectedMsg == actualMsg);
        }

        [Test]
        public void TrainHasLeftShouldReturnTrueIfEverythingIsOK()
        {
            station.NewArrivalOnBoard("Sofia -> Burgas");
            station.NewArrivalOnBoard("Sofia -> Gorna Oryahovica");
            station.NewArrivalOnBoard("Dragoman -> Plovdiv");
            station.TrainHasArrived("Sofia -> Burgas");
            station.TrainHasArrived("Sofia -> Gorna Oryahovica");
            Assert.IsTrue(station.TrainHasLeft("Sofia -> Burgas"));
            Assert.IsTrue(station.TrainHasLeft("Sofia -> Gorna Oryahovica"));
            Assert.IsFalse(station.DepartureTrains.Contains("Sofia -> Gorna Oryahovica"));
            Assert.IsFalse(station.DepartureTrains.Contains("Sofia -> Burgas"));
        }

        [Test]
        public void TrainHasArrivedShouldReturnFalseOfTheRowIsIncorrect()
        {
            station.NewArrivalOnBoard("Sofia -> Burgas");
            station.NewArrivalOnBoard("Sofia -> Gorna Oryahovica");
            station.NewArrivalOnBoard("Dragoman -> Plovdiv");
            station.TrainHasArrived("Sofia -> Burgas");
            station.TrainHasArrived("Sofia -> Gorna Oryahovica");
            station.TrainHasArrived("Dragoman -> Plovdiv");
            Assert.IsFalse(station.TrainHasLeft("Sofia -> Gorna Oryahovica"));
            Assert.IsFalse(station.TrainHasLeft("Dragoman -> Plovdiv"));
            Assert.IsTrue(station.DepartureTrains.Contains("Sofia -> Burgas"));
            int expectedCount = 3;
            Assert.That(expectedCount == station.DepartureTrains.Count);
        }
    }
}
