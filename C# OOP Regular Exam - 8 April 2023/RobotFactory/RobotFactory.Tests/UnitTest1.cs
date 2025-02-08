using NUnit.Framework;
using System.Linq;

namespace RobotFactory.Tests
{
    [TestFixture]
    public class Tests
    {
        private Factory factory;

        [SetUp]
        public void Setup()
        {
            factory = new("MyFactory", 4);
        }

        [Test]
        public void FactoryConstructorShouldInitializeCorrectly()
        {
            Factory factory11 = new("CustomFactory", 11);
            Assert.IsNotNull(factory11);
            Assert.IsNotNull(factory11.Supplements);
            Assert.IsNotNull(factory11.Robots);
            string expectedName = "CustomFactory";
            int expectedCapacity = 11;
            Assert.That(expectedName, Is.EqualTo(factory11.Name));
            Assert.That(expectedCapacity, Is.EqualTo(factory11.Capacity));
        }

        [TestCase("RobotFactory")]
        [TestCase("FutureReality")]
        [TestCase("SupplementsAndRobots2044")]
        public void FactoryNameShouldSetTheCorrectValue(string name)
        {
            factory = new(name, 14);
            Assert.That(name, Is.EqualTo(factory.Name));
        }

        [TestCase(1)]
        [TestCase(15)]
        [TestCase(101)]
        public void FactoryCapacityShouldSetTheCorrectValue(int capacity)
        {
            factory = new("SupplementsAndRobots", capacity);
            Assert.That(capacity, Is.EqualTo(factory.Capacity));
        }

        [Test]
        public void FactoryCollectionsShouldBeInitiallyEmpty()
        {
            int expectedCount = 0;
            Assert.That(expectedCount, Is.EqualTo(factory.Supplements.Count));
            Assert.That(expectedCount, Is.EqualTo(factory.Robots.Count));
        }

        [Test]
        public void ProduceRobotShouldReturnTheCorrectString()
        {
            string expectedMsg = "Produced --> Robot model: Kitchen Robot 14 IS: 10015, Price: 1200.00";
            string actualMsg = factory.ProduceRobot("Kitchen Robot 14", 1200, 10015);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            Assert.That(factory.Robots.Any(r => r.InterfaceStandard == 10015));
            Assert.That(factory.Robots.Any(r => r.Model == "Kitchen Robot 14"));
        }

        [Test]
        public void ProduceRobotShoulfReturnTheErrorMsgIfCapacityExceeded()
        {
            factory.ProduceRobot("Domestic Cleaner", 1750, 10033);
            factory.ProduceRobot("Hair Dryer", 140.33, 20075);
            factory.ProduceRobot("Dish Washer", 378.90, 40088);
            factory.ProduceRobot("Wash Dryer", 440, 20055);
            string expectedMsg = "The factory is unable to produce more robots for this production day!";
            int expectedCount = 4;
            Assert.That(expectedMsg, Is.EqualTo(factory.ProduceRobot("TestModel", 220, 90060)));
            Assert.That(expectedCount, Is.EqualTo(factory.Robots.Count));
            Assert.That(factory.Capacity, Is.EqualTo(factory.Robots.Count));
        }

        [Test]
        public void ProduceSupplementShouldReturnTheCorrectString()
        {
            string expectedMsg = "Supplement: Electronic Arm IS: 40088";
            string actualMsg = factory.ProduceSupplement("Electronic Arm", 40088);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            Assert.IsTrue(factory.Supplements.Any(s => s.InterfaceStandard == 40088));
            Assert.IsTrue(factory.Supplements.Any(s => s.Name == "Electronic Arm"));
        }

        [Test]
        public void UpgradeRobotShouldReturnTrueIfEverythingIsOK()
        {
            factory.ProduceRobot("Bath Cleaner", 255.90, 70045);
            factory.ProduceRobot("Plants Moisturizer", 144.75, 30013);
            factory.ProduceSupplement("Sprinkler", 30013);
            factory.ProduceSupplement("Steam Regulator", 70045);
            Robot bathRobot = factory.Robots.FirstOrDefault(r => r.Model == "Bath Cleaner");
            Supplement bathSupplement = factory.Supplements.FirstOrDefault(s => s.Name == "Steam Regulator");
            Assert.IsTrue(factory.UpgradeRobot(bathRobot, bathSupplement));
            Robot gardenRobot = factory.Robots.FirstOrDefault(r => r.Model == "Plants Moisturizer");
            Supplement gardenSupplement = factory.Supplements.FirstOrDefault(s => s.Name == "Sprinkler");
            Assert.IsTrue(factory.UpgradeRobot(gardenRobot, gardenSupplement));
            Assert.IsTrue(gardenRobot.Supplements.Contains(gardenSupplement));
            Assert.IsTrue(bathRobot.Supplements.Contains(bathSupplement));
        }

        [Test]
        public void UpgradeRobotShouldReturnFalseIfRobotContainsSuchSupplement()
        {
            factory.ProduceRobot("Electronic Vacuum Cleaner", 770.70, 10044);
            factory.ProduceSupplement("Vacuum Cleaner Brush", 10044);
            Robot robot = factory.Robots.FirstOrDefault(r => r.Model == "Electronic Vacuum Cleaner");
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.Name == "Vacuum Cleaner Brush");
            factory.UpgradeRobot(robot, supplement);
            Assert.IsFalse(factory.UpgradeRobot(robot, supplement));
            int expectedCount = 1;
            Assert.That(expectedCount, Is.EqualTo(robot.Supplements.Count));
        }

        [Test]
        public void UpgradeRobotShouldReturnFalseIfInterfaceStandardsDiffer()
        {
            factory.ProduceRobot("Electronic Vacuum Cleaner", 770.70, 10044);
            factory.ProduceSupplement("Vacuum Cleaner Brush", 20044);
            Robot robot = factory.Robots.FirstOrDefault(r => r.Model == "Electronic Vacuum Cleaner");
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.Name == "Vacuum Cleaner Brush");
            Assert.IsFalse(factory.UpgradeRobot(robot, supplement));
            Assert.IsFalse(robot.Supplements.Any());
        }

        [Test]
        public void SellRobotShouldReturnTheCorrectRobot()
        {
            factory.ProduceRobot("Domestic Cleaner", 1750, 10033);
            factory.ProduceRobot("Hair Dryer", 140.33, 20075);
            factory.ProduceRobot("Dish Washer", 378.90, 40088);
            factory.ProduceRobot("Wash Dryer", 440, 20055);
            double budget = 400;
            Robot expectedRobot = factory.Robots.OrderByDescending(r => r.Price).FirstOrDefault(r => r.Price <= budget);
            Robot actualRobot = factory.SellRobot(budget);
            Assert.That(expectedRobot, Is.EqualTo(actualRobot));
        }
    }
}