using NUnit.Framework;

namespace VendingRetail.Tests
{
    public class Tests
    {
        private CoffeeMat coffeeMat;

        [SetUp]
        public void Setup()
        {
            coffeeMat = new(250, 5);
            coffeeMat.AddDrink("Coffee", 0.70);
            coffeeMat.AddDrink("Milk", 0.80);
            coffeeMat.AddDrink("Tea", 0.60);
        }

        [Test]
        public void ConstructorShouldWorkCorrectly()
        {
            CoffeeMat coffeeMat = new CoffeeMat(400, 4);
            int expectedCapacity = 400;
            int expectedCount = 4;
            int actualCapacity = coffeeMat.WaterCapacity;
            int actualCount = coffeeMat.ButtonsCount;
            Assert.AreEqual(expectedCapacity, actualCapacity);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void CoffeematShouldNotBeNull()
        {
            CoffeeMat coffeeMat = new CoffeeMat(400, 4);
            Assert.IsNotNull(coffeeMat);
        }

        [Test]
        public void WaterCapacityPropertyShouldWorkCorrectly()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 10);
            int expectedWaterCapacity = 500;
            int actualCapacity = coffeeMat.WaterCapacity;
            Assert.AreEqual(expectedWaterCapacity, actualCapacity);
        }

        [Test]
        public void ButtonsCountShouldBeCorrect()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 10);
            int expectedCount = 10;
            int actualCount = coffeeMat.ButtonsCount;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void AddDrinkShouldReturnTrueIfOK()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 10);
            Assert.IsTrue(coffeeMat.AddDrink("Mocca", 1.50));
        }

        [Test]
        public void AddDrinkShouldReturnFalseIfNameExists()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 10);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            Assert.IsFalse(coffeeMat.AddDrink("WienerCoffee", 2.10));
        }

        [Test]
        public void AddingDrinkShouldReturnFalseIfCapacityExceeded()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.AddDrink("DoubleCoffee", 1.40);
            Assert.IsFalse(coffeeMat.AddDrink("NormalCoffee", 0.80));
        }

        [Test]
        public void BuyDrinkShouldReturnCorrectMessageIfNameDoesntExist()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.AddDrink("DoubleCoffee", 1.40);
            coffeeMat.FillWaterTank();

            string actualMsg = coffeeMat.BuyDrink("Coca-cola");
            string expectedMsg = "Coca-cola is not available!";

            Assert.AreEqual(expectedMsg, actualMsg);
        }

        [Test]
        public void BuyDrinkShouldReturnCorrectMessageIfEverythingIsOK()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.AddDrink("DoubleCoffee", 1.40);
            coffeeMat.FillWaterTank();

            string expectedMsg = "Your bill is 1.10$";
            string actualMsg = coffeeMat.BuyDrink("Cappuccino");
            Assert.AreEqual(expectedMsg, actualMsg);
        }

        [Test]
        public void BuyDrinkShouldReturnCorrectMessageIfOutOfWater()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);

            string expectedMsg = "CoffeeMat is out of water!";
            string actualMsg = coffeeMat.BuyDrink("Mocca");
            Assert.AreEqual(expectedMsg, actualMsg);
        }

        [Test]
        public void FillWaterTankShouldReturnCorrectMessageIfAllreadyFull()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);
            coffeeMat.FillWaterTank();

            string expectedMsg = "Water tank is already full!";
            string actualMsg = coffeeMat.FillWaterTank();
            Assert.AreEqual(expectedMsg, actualMsg);
        }

        [Test]
        public void BuyDrinkShouldReturnCorrectMessageIfWaterLevelIsBelow80()
        {
            CoffeeMat coffeeMat = new CoffeeMat(300, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.10);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.AddDrink("DoubleCoffee", 1.40);
            coffeeMat.FillWaterTank();

            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("WienerCoffee");

            string expectedMsg = "CoffeeMat is out of water!";
            string actualMsg = coffeeMat.BuyDrink("DoubleCoffee");
            Assert.AreEqual(expectedMsg, actualMsg);
        }

        [Test]
        public void CollectIncomeShouldReturnTheCorrectDouble()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.15);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.FillWaterTank();

            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("Cappuccino");
            coffeeMat.BuyDrink("WienerCoffee");

            double expected = 5.15;
            double actual = coffeeMat.CollectIncome();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CollectIncomeShouldZeroTheTotalIncome()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.15);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.FillWaterTank();

            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("Cappuccino");
            coffeeMat.BuyDrink("WienerCoffee");
            coffeeMat.CollectIncome();

            coffeeMat.BuyDrink("Mocca");
            double expectedSum = 1.50;
            double actualSum = coffeeMat.CollectIncome();

            Assert.AreEqual(expectedSum, actualSum);
        }

        [Test]
        public void EmptyTankShouldNotChangeTheIncome()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.15);
            coffeeMat.AddDrink("WienerCoffee", 2.50);

            coffeeMat.BuyDrink("Mocca");
            double expectedSum = 0;
            double actualSum = coffeeMat.CollectIncome();
            Assert.AreEqual(expectedSum, actualSum);
        }

        [Test]
        public void IncomePropertyShouldAcceptTheCorrectValue()
        {
            CoffeeMat coffeeMat = new CoffeeMat(500, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.15);
            coffeeMat.AddDrink("WienerCoffee", 2.50);
            coffeeMat.FillWaterTank();

            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("Cappuccino");
            coffeeMat.BuyDrink("WienerCoffee");

            double expected = 5.15;
            double actual = coffeeMat.Income;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FillTankShouldReturnTheCorrectMessage()
        {
            CoffeeMat coffeeMat = new CoffeeMat(300, 4);
            coffeeMat.AddDrink("Mocca", 1.50);
            coffeeMat.AddDrink("Cappuccino", 1.15);
            coffeeMat.AddDrink("WienerCoffee", 2.50);

            coffeeMat.FillWaterTank();

            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("Mocca");
            coffeeMat.BuyDrink("Mocca");

            string expectedMsg = "Water tank is filled with 240ml";
            string actualMsg = coffeeMat.FillWaterTank();
            Assert.AreEqual(expectedMsg, actualMsg);
        }
    }
}
