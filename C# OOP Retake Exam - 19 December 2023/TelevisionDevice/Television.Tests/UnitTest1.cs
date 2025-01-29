namespace Television.Tests
{
    using System;
    using NUnit.Framework;
    public class Tests
    {
        private TelevisionDevice device;

        [SetUp]
        public void Setup()
        {
            device = new("Finlux", 459.99, 60, 45);
        }

        [Test]
        public void DeviceConstructorShouldInitializeCorrectly()
        {
            TelevisionDevice device1 = new("Crown", 359.90, 55, 45);
            Assert.IsNotNull(device1);
            string expectedBrand = "Crown";
            double expectedPrice = 359.90;
            int expectedWidth = 55;
            int expectedHeight = 45;
            Assert.That(expectedBrand == device1.Brand);
            Assert.That(expectedPrice == device1.Price);
            Assert.That(expectedWidth == device1.ScreenWidth);
            Assert.That(expectedHeight == device1.ScreenHeigth);
        }

        [Test]
        public void BrandShouldSetTheCorrectValue()
        {
            string expectedBrand = "Finlux";
            Assert.That(expectedBrand == device.Brand);
        }

        [Test]
        public void PriceShouldBeSetCorrectly()
        {
            double expectedPrice = 459.99;
            Assert.That(expectedPrice == device.Price);
        }

        [Test]
        public void ScreenWidthShouldBeSetCorrectly()
        {
            int expectedWidth = 60;
            Assert.That(expectedWidth == device.ScreenWidth);
        }

        [Test]
        public void ScreenHeightShouldBeSetCorrectly()
        {
            int expectedHeight = 45;
            Assert.That(expectedHeight == device.ScreenHeigth);
        }

        [Test]
        public void CurrentChannelShouldBeInitiallySetToZero()
        {
            int expectedValue = 0;
            Assert.That(expectedValue == device.CurrentChannel);
        }

        [Test]
        public void VolumeShouldBeInitiallySetTo13()
        {
            int expectedValue = 13;
            Assert.That(expectedValue == device.Volume);
        }

        [Test]
        public void IsMutedShouldBeInitiallySetToFalse()
        {
            Assert.IsFalse(device.IsMuted);
            device.MuteDevice();
            Assert.IsTrue(device.IsMuted);
        }

        [Test]
        public void SwitchOnMethodShouldReturnTheCorrectString()
        {
            device.MuteDevice();
            string expectedMsg = "Cahnnel 0 - Volume 13 - Sound Off";
            string actualMsg = device.SwitchOn();
            Assert.That(expectedMsg == actualMsg);
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(3)]
        [TestCase(111)]
        public void ChangeChannelMethodShouldReturnTheCorrectNewChannel(int channel)
        {
            int expectedValue = channel;
            Assert.That(expectedValue == device.ChangeChannel(channel));
        }

        [TestCase(-1)]
        [TestCase(-1112)]
        [TestCase(-14)]
        public void ChangeChannelMethodShouldThrowIfValueIsNegative(int channel)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => device.ChangeChannel(channel), "Invalid key!");
        }

        [TestCase(13)]
        [TestCase(10)]
        [TestCase(3)]
        public void VolumeChangeMethodShouldCalculateTheCorrectValue(int units)
        {
            int expectedValue = device.Volume + units;
            device.VolumeChange("UP", units);
            int actualValue = device.Volume;
            Assert.That(expectedValue == actualValue);
        }

        [TestCase(97)]
        [TestCase(98)]
        [TestCase(112)]
        public void VolumeChangeShouldSetTheVolumeTo100IfAboveMaxLevel(int units)
        {
            device.VolumeChange("UP", units);
            int expectedValue = 100;
            int actualValue = device.Volume;
            Assert.That(expectedValue == actualValue);
        }

        [Test]
        public void VolumeChangeShouldReturnTheCorrectMessage()
        {
            string expectedMsg = "Volume: 30";
            string actualMsg = device.VolumeChange("UP", 17);
            Assert.That(expectedMsg == actualMsg);
        }

        [Test]
        public void VolumeChangeShouldDecreaseTheVolumeCorrectly()
        {
            device.VolumeChange("UP", 17);
            device.VolumeChange("DOWN", 5);
            int expectedVolume = 25;
            int actualVolume = device.Volume;
            Assert.That(expectedVolume == actualVolume);
        }

        [TestCase(13)]
        [TestCase(14)]
        [TestCase(25)]
        public void VolumeChangeCannotSetVolumeBelowZero(int units)
        {
            device.VolumeChange("DOWN", units);
            int expectedValue = 0;
            int actualValue = device.Volume;
            Assert.That(expectedValue == actualValue);
        }

        [Test]
        public void VolumeChangeShouldReturnTheCorrectMsgIfDirectionIsDown()
        {
            string expectedMsg = "Volume: 5";
            string actualMsg = device.VolumeChange("DOWN", 8);
        }

        [Test]
        public void MuteDeviceShouldToggleAndReturnTrueOrFalse()
        {
            Assert.IsTrue(device.MuteDevice());
            Assert.IsFalse(device.MuteDevice());
        }

        [Test]
        public void SwitchOnShouldReturnTheCorrectMessageAfterUsingOtherMethods()
        {
            device.VolumeChange("UP", 7);
            device.VolumeChange("DOWN", 10);
            device.MuteDevice();
            device.MuteDevice();
            device.ChangeChannel(10);
            string expectedMsg = "Cahnnel 10 - Volume 10 - Sound On";
            string actualMsg = device.SwitchOn();
            Assert.That(expectedMsg == actualMsg);
        }

        [Test]
        public void ToStringMethodShouldReturnTheCorrectString()
        {
            string expectedMsg = "TV Device: Finlux, Screen Resolution: 60x45, Price 459.99$";
            string actualMsg = device.ToString();
            Assert.That(expectedMsg == actualMsg);
        }
    }
}
