using NUnit.Framework;
using System.Linq;

namespace RobotFactory.Tests
{
    public class Tests
    {
        private Supplement supplement1;
        private Supplement supplement2;

        private Robot robot1;
        private Robot robot2;

        private Factory factory;

        [SetUp]
        public void Setup()
        {
            supplement1 = new Supplement("ElectronicArm", 810110);
            supplement2 = new Supplement("DishWasher", 555310);

            robot1 = new("Arm110", 330.50, 810110);
            robot2 = new("Washer515", 255.40, 555310);

            factory = new("MyFactory85", 3);
        }

        [Test]
        public void SupplementConstructorShouldInitializeCorrectly()
        {
            string expectedName = "ElectronicArm";
            int expectedStandard = 810110;

            Assert.IsNotNull(supplement1);
            Assert.That(expectedName == supplement1.Name);
            Assert.That(expectedStandard == supplement1.InterfaceStandard);
        }

        [Test]
        public void SupplementToStringShouldReturnTheCorrectString()
        {
            string expectedOutput = "Supplement: DishWasher IS: 555310";
            string actualOutput = supplement2.ToString();
            Assert.That(expectedOutput == actualOutput);
        }

        [Test]
        public void RobotConstructorShouldSetTheCorrectValues()
        {
            string expectedName = "Arm110";
            double expectedPrice = 330.50;
            int expectedInterface = 810110;

            Assert.IsNotNull(robot1);
            Assert.That(expectedName == robot1.Model);
            Assert.That(expectedPrice == robot1.Price);
            Assert.That(expectedInterface == robot1.InterfaceStandard);
        }

        [Test]
        public void CollectionOfSupplementsShouldNotBeNull()
        {
            Assert.IsNotNull(robot1.Supplements);
            Assert.IsNotNull(robot2.Supplements);
        }

        [Test]
        public void RobotToStringShouldReturnTheCorrectString()
        {
            string expectedFirstString = "Robot model: Arm110 IS: 810110, Price: 330.50";
            string expectedSecondString = "Robot model: Washer515 IS: 555310, Price: 255.40";
            string actualFirstString = robot1.ToString();
            string actualSecondString = robot2.ToString();
            Assert.That(expectedFirstString == actualFirstString);
            Assert.That(expectedSecondString == actualSecondString);
        }

        [Test]
        public void FactoryConstructorShouldInitializeCorrectValues()
        {
            string expectedName = "MyFactory85";
            int expectedCapacity = 3;
            Assert.IsNotNull(factory);
            Assert.That(expectedName == factory.Name);
            Assert.That(expectedCapacity == factory.Capacity);
        }

        [Test]
        public void CollectionsInitializedByTheConstructorShouldNotBeNull()
        {
            Assert.IsNotNull(factory.Supplements);
            Assert.IsNotNull(factory.Robots);
        }

        [Test]
        public void ProduceRobotShouldIncreaseTheCountOfRobots()
        {
            factory.ProduceRobot("Washer", 230, 515010);
            factory.ProduceRobot("Cooker", 250, 313330);
            int expectedCount = 2;
            int actualCount = factory.Robots.Count;
            Assert.That(expectedCount == actualCount);
        }

        [Test]
        public void RobotsShouldContainTheProducedRobot()
        {
            factory.ProduceRobot("Washer", 230, 515010);
            Robot robot = factory.Robots.FirstOrDefault(r => r.Model == "Washer");
            Assert.IsNotNull(robot);
        }

        [Test]
        public void ProduceRobotShouldReturnTheCorrectString()
        {
            string expected = "Produced --> Robot model: Arm110 IS: 810110, Price: 330.50";
            string actual = factory.ProduceRobot("Arm110", 330.50, 810110);
            Assert.That(expected == actual);
        }

        [Test]
        public void ProduceRobotShouldReturnTheCorrectStringIfCapacityExceeded()
        {
            factory.ProduceRobot("Washer333", 110, 805030);
            factory.ProduceRobot("Dryer", 170, 515005);
            factory.ProduceRobot("Cleaner", 230, 115000);
            string expected = "The factory is unable to produce more robots for this production day!";
            string actual = factory.ProduceRobot("Test", 85, 110320);
            Assert.That(expected == actual);
            Assert.That(3 == factory.Robots.Count);
        }

        [Test]
        public void ProduceSupplementShouldIncreaseTheCount()
        {
            factory.ProduceSupplement("supplement1", 515550);
            factory.ProduceSupplement("supplement2", 515510);
            int expectedCount = 2;
            int actualCount = factory.Supplements.Count;
            Assert.That(expectedCount == actualCount);
        }

        [Test]
        public void SupplementsShouldContainTheProducedSupplement()
        {
            factory.ProduceSupplement("supplement1", 515550);
            factory.ProduceSupplement("supplement2", 515510);
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.InterfaceStandard == 515550);
            Assert.IsNotNull(supplement);
        }

        [Test]
        public void ProduceSupplementShouldReturnTheCorrectString()
        {
            string expectedOutput = "Supplement: DishWasher IS: 555310";
            string actualOutput = factory.ProduceSupplement("DishWasher", 555310);
            Assert.That(expectedOutput == actualOutput);
        }

        [Test]
        public void UpgradeRobotShouldReturnFalseOfInterfaceNotCorresponding()
        {
            factory.ProduceRobot("VacuumCleaner", 340, 888110);
            factory.ProduceSupplement("Brush", 888220);
            Robot robot = factory.Robots.FirstOrDefault(v => v.Model == "VacuumCleaner");
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.Name == "Brush");
            Assert.IsFalse(factory.UpgradeRobot(robot, supplement));
        }

        [Test]
        public void UpgradeRobotShouldReturnFalseIfAlreadyUpgraded()
        {
            factory.ProduceRobot("VacuumCleaner", 340, 888110);
            factory.ProduceSupplement("Brush", 888110);
            factory.ProduceSupplement("AnotherBrush", 888110);
            Robot robot = factory.Robots.FirstOrDefault(v => v.Model == "VacuumCleaner");
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.Name == "Brush");
            Supplement supplement2 = factory.Supplements.FirstOrDefault(s => s.Name == "AnotherBrush");
            factory.UpgradeRobot(robot, supplement);
            Assert.IsFalse(factory.UpgradeRobot(robot, supplement));
        }

        [Test]
        public void UpgradeRobotShouldAddSupplementToTheCollectionInRobot()
        {
            factory.ProduceRobot("VacuumCleaner", 340, 888110);
            factory.ProduceSupplement("Brush", 888110);
            Robot robot = factory.Robots.FirstOrDefault(v => v.Model == "VacuumCleaner");
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.Name == "Brush");
            factory.UpgradeRobot(robot, supplement);
            int expectedCount = 1;
            int actualCount = robot.Supplements.Count;
            Assert.That(expectedCount == actualCount);
        }

        [Test]
        public void UpgrageRobotShouldReturnTrueIfSupplementAddedSuccessfully()
        {
            factory.ProduceRobot("VacuumCleaner", 340, 888110);
            factory.ProduceSupplement("Brush", 888110);
            Robot robot = factory.Robots.FirstOrDefault(v => v.Model == "VacuumCleaner");
            Supplement supplement = factory.Supplements.FirstOrDefault(s => s.Name == "Brush");
            Assert.IsTrue(factory.UpgradeRobot(robot, supplement));
        }

        [TestCase(200)]
        public void SellRobotShouldReturnTheCorrectObject(double price)
        {
            factory.ProduceRobot("Washer333", 110, 805030);
            factory.ProduceRobot("Dryer", 170, 515005);
            factory.ProduceRobot("Cleaner", 230, 115000);
            Robot dryer = factory.Robots.OrderByDescending(r => r.Price)
                .FirstOrDefault(d => d.Price <= price);

            Robot actual = factory.SellRobot(price);
            Assert.That(dryer == actual);
        }
    }
}