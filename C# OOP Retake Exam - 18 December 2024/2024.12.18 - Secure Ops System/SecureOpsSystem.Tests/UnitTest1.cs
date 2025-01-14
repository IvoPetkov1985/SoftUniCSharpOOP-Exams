using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SecureOpsSystem.Tests
{
    [TestFixture]
    public class SecureHubTests
    {
        private SecurityTool tool1;
        private SecurityTool tool2;
        private SecureHub hub;

        [SetUp]
        public void Setup()
        {
            hub = new(3);
            tool1 = new("Eset", "AntiMalware", 55);
            tool2 = new("Kaspersky", "Antivirus", 99.9);
        }

        [Test]
        public void ConstructorShouldInitializeCorrectly()
        {
            SecureHub hub = new(10);
            Assert.IsNotNull(hub);
            int expectedCapacity = 10;
            int actualCapacity = hub.Capacity;
            Assert.That(expectedCapacity, Is.EqualTo(actualCapacity));
            Assert.That(hub.Tools, Is.Not.Null);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(17)]
        public void CapacityShouldBeSetCorrectly(int capacity)
        {
            hub = new(capacity);
            Assert.That(capacity, Is.EqualTo(hub.Capacity));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-15)]
        public void CapacityShouldThrowIfValueIsZeroOrNegative(int capacity)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => hub = new(capacity), "Capacity must be greater than 0.");
        }

        [Test]
        public void ToolsShouldBeEmptyInitially()
        {
            int expectedCount = 0;
            Assert.That(expectedCount, Is.EqualTo(hub.Tools.Count));
        }

        [Test]
        public void ToolsShouldDisplayTheCorrectCount()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            int expectedCount = 2;
            Assert.That(expectedCount, Is.EqualTo(hub.Tools.Count));
        }

        [Test]
        public void AddToolShouldReturnTheCorrectMessage()
        {
            string expectedMsg = "Security Tool Eset added successfully.";
            string actualMsg = hub.AddTool(tool1);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }

        [Test]
        public void AddToolShouldReturnMessageIfToolExists()
        {
            hub.AddTool(tool1);
            SecurityTool toolEset = new("Eset", "AntiVirus", 90);
            string expectedMsg = "Security Tool Eset already exists in the hub.";
            string actualMsg = hub.AddTool(toolEset);
            int expectedCount = 1;
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            Assert.That(expectedCount, Is.EqualTo(hub.Tools.Count));
        }

        [Test]
        public void AddToolShouldReturnMessageIfCapacityIsFull()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            SecurityTool tool3 = new("AVG", "AntiVirus", 75);
            SecurityTool tool4 = new("F-Secure", "AllInOne", 68);
            hub.AddTool(tool3);
            string expectedMsg = "Secure Hub is at full capacity.";
            string actualMsg = hub.AddTool(tool4);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            int expectedCount = 3;
            Assert.That(expectedCount, Is.EqualTo(hub.Tools.Count));
        }

        [Test]
        public void AddedToolShouldExistInTheCollection()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            Assert.IsTrue(hub.Tools.Contains(tool1));
            Assert.IsTrue(hub.Tools.Contains(tool2));
        }

        [Test]
        public void RemoveToolShouldDecreaseCount()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            SecurityTool tool3 = new("NetworkSecure", "AntiSpyware", 98);
            hub.AddTool(tool3);
            hub.RemoveTool(tool1);
            int expectedCount = 2;
            int actualCount = hub.Tools.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void AfterToolRemovalItShouldNotBePresentInCollection()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            SecurityTool tool3 = new("NetworkSecure", "AntiSpyware", 98);
            hub.AddTool(tool3);
            hub.RemoveTool(tool1);
            Assert.IsFalse(hub.Tools.Contains(tool1));
        }

        [Test]
        public void RemoveToolShouldReturnTrueOrFalse()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            SecurityTool tool3 = new("NetworkSecure", "AntiSpyware", 98);
            Assert.IsTrue(hub.RemoveTool(tool2));
            Assert.IsFalse(hub.RemoveTool(tool3));
        }

        [Test]
        public void DeployToolShouldReturnTheCorrectTool()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            Assert.That(tool1, Is.EqualTo(hub.DeployTool("Eset")));
            Assert.That(tool2, Is.EqualTo(hub.DeployTool("Kaspersky")));
        }

        [Test]
        public void DeployToolShouldReturnNullIfToolNotExist()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            Assert.IsNull(hub.DeployTool("NetworkSecure"));
        }

        [Test]
        public void DeployToolShouldDecreaseTheCountAndRemoveFromCollection()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            hub.DeployTool("Kaspersky");
            Assert.IsFalse(hub.Tools.Contains(tool2));
            int expectedCount = 1;
            Assert.That(expectedCount, Is.EqualTo(hub.Tools.Count));
        }

        [Test]
        public void SystemReportShouldReturnTheCorrectString()
        {
            hub.AddTool(tool1);
            hub.AddTool(tool2);
            SecurityTool tool3 = new("AVG", "AntiVirus", 78);
            hub.AddTool(tool3);

            StringBuilder expected = new();
            expected.AppendLine("Secure Hub Report:");
            expected.AppendLine("Available Tools: 3");
            expected.AppendLine(tool2.ToString());
            expected.AppendLine(tool3.ToString());
            expected.AppendLine(tool1.ToString());

            string expectedOutput = expected.ToString().TrimEnd();
            string actualOutput = hub.SystemReport();
            Assert.That(expectedOutput, Is.EqualTo(actualOutput));
        }
    }
}
