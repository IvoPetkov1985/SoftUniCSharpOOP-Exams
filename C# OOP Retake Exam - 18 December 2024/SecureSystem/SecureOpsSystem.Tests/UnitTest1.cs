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
        private SecureHub hub1;
        private SecurityTool tool1;
        private SecurityTool tool2;
        private SecurityTool tool3;
        private SecurityTool tool4;

        [SetUp]
        public void SetUp()
        {
            hub1 = new(4);
            tool1 = new("Nod32", "Antivirus", 71);
            tool2 = new("AVG", "Antimalware", 75.5);
            tool3 = new("Kaspersky", "Antispyware", 91.7);
            tool4 = new("Windows Defender", "Antimalware", 88.8);
        }

        [Test]
        public void HubConstructorShouldInitializeCorrectly()
        {
            SecureHub hub = new(10);
            Assert.IsNotNull(hub);
            Assert.IsNotNull(hub.Tools);
            int expectedCapacity = 10;
            Assert.That(expectedCapacity == hub.Capacity);
        }

        [TestCase(1)]
        [TestCase(11)]
        [TestCase(27)]
        public void HubCapacityShouldSetTheCorrectValue(int capacity)
        {
            hub1 = new(capacity);
            Assert.That(capacity == hub1.Capacity);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-33)]
        public void HubCapacityShouldThrowIfValueIsNotPositive(int capacity)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => hub1 = new(capacity), "Capacity must be greater than 0.");
        }

        [Test]
        public void ToolsCollectionShouldBeInitiallyEmpty()
        {
            int initialCount = 0;
            Assert.That(initialCount == hub1.Tools.Count);
        }

        [Test]
        public void AddToolShouldReturnTheCorrectString()
        {
            hub1.AddTool(tool1);
            string expectedMsg = "Security Tool AVG added successfully.";
            string actualMsg = hub1.AddTool(tool2);
            Assert.That(expectedMsg == actualMsg);
            Assert.IsTrue(hub1.Tools.Contains(tool1));
            Assert.IsTrue(hub1.Tools.Contains(tool2));
            int expectedCount = 2;
            Assert.That(expectedCount == hub1.Tools.Count);
        }

        [Test]
        public void AddToolShouldReturnTheCorrectStringIfNameDuplicated()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool2);
            SecurityTool tool5 = new("AVG", "AntiAll", 10.3);
            string expectedMsg = "Security Tool AVG already exists in the hub.";
            string actualMsg = hub1.AddTool(tool5);
            Assert.That(expectedMsg == actualMsg);
        }

        [Test]
        public void AddToolShouldReturnTheCorrectStringIfCapacityIsFull()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool2);
            hub1.AddTool(tool3);
            hub1.AddTool(tool4);
            SecurityTool tool5 = new("Kaspiyski", "AntiAll", 10.3);
            string expectedMsg = "Secure Hub is at full capacity.";
            string actualMsg = hub1.AddTool(tool5);
            Assert.That(expectedMsg == actualMsg);
            Assert.IsFalse(hub1.Tools.Contains(tool5));
        }

        [Test]
        public void RemoveToolShouldWorkCorrectly()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool4);
            Assert.IsTrue(hub1.RemoveTool(tool1));
            Assert.IsFalse(hub1.RemoveTool(tool2));
        }

        [Test]
        public void RemoveToolShouldDecrementTheCount()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool2);
            hub1.AddTool(tool3);
            hub1.AddTool(tool4);
            hub1.RemoveTool(tool3);
            int expectedCount = 3;
            int actualCount = hub1.Tools.Count;
            Assert.That(expectedCount == actualCount);
            Assert.IsFalse(hub1.Tools.Contains(tool3));
        }

        [Test]
        public void DeployToolShouldReturnTheCorrectTool()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool2);
            hub1.AddTool(tool3);
            hub1.AddTool(tool4);
            Assert.That(tool2 == hub1.DeployTool("AVG"));
        }

        [Test]
        public void DeployToolShouldDecrementTheCountAndRemoveTheSpecifiedTool()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool2);
            hub1.AddTool(tool3);
            hub1.AddTool(tool4);
            hub1.DeployTool("Nod32");
            hub1.DeployTool("AVG");
            Assert.IsFalse(hub1.Tools.Contains(tool1));
            Assert.IsFalse(hub1.Tools.Contains(tool2));
            int expectedCount = 2;
            Assert.That(expectedCount == hub1.Tools.Count);
        }

        [Test]
        public void DeployToolShouldReturnNullIfNameNotInTheList()
        {
            hub1.AddTool(tool1);
            hub1.AddTool(tool2);
            hub1.AddTool(tool3);
            Assert.IsNull(hub1.DeployTool("Norton AV"));
        }

        [Test]
        public void SystemReportShouldReturnTheCorrectString()
        {
            hub1.AddTool(tool2);
            hub1.AddTool(tool3);
            hub1.AddTool(tool4);
            StringBuilder builder = new();
            builder.AppendLine("Secure Hub Report:");
            builder.AppendLine("Available Tools: 3");
            builder.AppendLine(tool3.ToString());
            builder.AppendLine(tool4.ToString());
            builder.AppendLine(tool2.ToString());
            string expected = builder.ToString().TrimEnd();
            string actual = hub1.SystemReport();
            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}
