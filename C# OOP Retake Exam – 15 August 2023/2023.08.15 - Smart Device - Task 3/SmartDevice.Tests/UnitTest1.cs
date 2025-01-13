namespace SmartDevice.Tests
{
    using NUnit.Framework;
    using System;
    using System.Text;

    public class Tests
    {
        private Device smartphone;

        [SetUp]
        public void Setup()
        {
            smartphone = new(2000);
        }

        [Test]
        public void DeviceConstructorShouldInitializeDorrectly()
        {
            Assert.IsNotNull(smartphone);
            Assert.IsNotNull(smartphone.Applications);
        }

        [TestCase(900)]
        [TestCase(3500)]
        [TestCase(50000)]
        public void DeciceConstructorShouldSetTheCorrectValues(int capacity)
        {
            smartphone = new(capacity);
            int expectedCapacity = capacity;
            int actualCapacity = smartphone.MemoryCapacity;
            int actualAvailableMemory = smartphone.AvailableMemory;
            int expectedPhotosCount = 0;
            int actualPhotosCount = smartphone.Photos;
            int extectedAppsCount = 0;
            int actualAppsCount = smartphone.Applications.Count;
            Assert.That(expectedCapacity == actualCapacity);
            Assert.That(expectedCapacity == actualAvailableMemory);
            Assert.That(expectedPhotosCount == actualPhotosCount);
            Assert.That(extectedAppsCount == actualAppsCount);
        }

        [Test]
        public void TakePhotoShouldReturnTrueIfEnoughCapacity()
        {
            Assert.IsTrue(smartphone.TakePhoto(98));
        }

        [Test]
        public void TakePhotoShouldDecreaseAvailableMemoryAndIncrementPhotosCount()
        {
            smartphone.TakePhoto(96);
            int expectedMemory = 1904;
            int actualMemory = smartphone.AvailableMemory;
            Assert.That(expectedMemory == actualMemory);
            int expectedPhotoCount = 1;
            Assert.That(expectedPhotoCount == smartphone.Photos);
        }

        [Test]
        public void TakePhotoShouldReturnFalseIfNotEnoughMemory()
        {
            smartphone = new(200);
            smartphone.TakePhoto(50);
            smartphone.TakePhoto(55);
            smartphone.TakePhoto(55);
            int expectedCount = 3;
            int actualCount = smartphone.Photos;
            int expectedMemory = 40;
            int actualAvailableMemory = smartphone.AvailableMemory;
            Assert.That(expectedCount == actualCount);
            Assert.IsFalse(smartphone.TakePhoto(68));
            Assert.That(expectedMemory == actualAvailableMemory);
        }

        [Test]
        public void InstallAppShouldWorkCorrectly()
        {
            smartphone.InstallApp("Rally III", 510);
            smartphone.InstallApp("Instagram", 500);
            int expectedAppsCount = 2;
            int actualAppsCount = smartphone.Applications.Count;
            int expectedMemory = 990;
            int actualMemory = smartphone.AvailableMemory;
            Assert.That(smartphone.Applications.Contains("Instagram"));
            Assert.That(smartphone.Applications.Contains("Rally III"));
            Assert.That(expectedAppsCount == actualAppsCount);
            Assert.That(expectedMemory == actualMemory);
        }

        [Test]
        public void InstallAppShouldReturnTheConfirmationMessageIfOperationSuccessful()
        {
            string expectedMessage = "Instagram is installed successfully. Run application?";
            string actualMessage = smartphone.InstallApp("Instagram", 540);
            Assert.That(expectedMessage, Is.EqualTo(actualMessage));
        }

        [Test]
        public void InstallAppShouldReturnFailedMessageIfNotEnoughMemory()
        {
            smartphone.InstallApp("Viber", 700);
            smartphone.InstallApp("Skype", 550);
            smartphone.InstallApp("Tumblr", 750);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => smartphone.InstallApp("Facebook", 750), "Not enough available memory to install the app.");
        }

        [Test]
        public void FormatDeviceShouldRevertTheFactorySettings()
        {
            smartphone.TakePhoto(44);
            smartphone.TakePhoto(66);
            smartphone.TakePhoto(50);
            smartphone.TakePhoto(40);
            smartphone.TakePhoto(77);
            smartphone.TakePhoto(29);

            smartphone.InstallApp("Solitare", 110);
            smartphone.InstallApp("WhatsApp", 330);

            smartphone.FormatDevice();

            int expectedAvailableMemory = 2000;
            int expectedPhotosCount = 0;
            int expectedAppsCount = 0;

            Assert.That(expectedAvailableMemory == smartphone.MemoryCapacity);
            Assert.That(expectedPhotosCount == smartphone.Photos);
            Assert.That(expectedAppsCount == smartphone.Applications.Count);
            Assert.IsNotNull(smartphone.Applications);
        }

        [Test]
        public void AfterFormatDeviceApplicationsShouldNotContainAnything()
        {
            smartphone.InstallApp("Solitare", 110);
            smartphone.InstallApp("WhatsApp", 330);
            smartphone.FormatDevice();
            Assert.IsFalse(smartphone.Applications.Contains("Solitare"));
            Assert.IsFalse(smartphone.Applications.Contains("WhatsApp"));
        }

        [Test]
        public void GetStatusShouldReturnTheCorrectString()
        {
            smartphone.TakePhoto(50);
            smartphone.TakePhoto(52);
            smartphone.TakePhoto(48);

            smartphone.InstallApp("Sofascore", 100);
            smartphone.InstallApp("ChatGPT", 250);
            smartphone.InstallApp("Tumblr", 200);
            smartphone.InstallApp("Instagram", 300);

            StringBuilder builder = new();
            builder.AppendLine("Memory Capacity: 2000 MB, Available Memory: 1000 MB");
            builder.AppendLine("Photos Count: 3");
            builder.AppendLine("Applications Installed: Sofascore, ChatGPT, Tumblr, Instagram");

            string expectedReport = builder.ToString().TrimEnd();
            string actualReport = smartphone.GetDeviceStatus();

            Assert.That(expectedReport, Is.EqualTo(actualReport));
        }
    }
}
