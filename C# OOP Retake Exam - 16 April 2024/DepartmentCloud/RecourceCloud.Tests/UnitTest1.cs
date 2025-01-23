using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RecourceCloud.Tests
{
    public class Tests
    {
        private DepartmentCloud cloud;

        [SetUp]
        public void Setup()
        {
            cloud = new();
        }

        [Test]
        public void ConstructorShouldWorkCorrectly()
        {
            DepartmentCloud cloud = new();
            Assert.IsNotNull(cloud);
            Assert.IsNotNull(cloud.Tasks);
            Assert.IsNotNull(cloud.Resources);
        }

        [Test]
        public void CollectionsShouldHaveNoElementsUponInitialization()
        {
            int expectedTasksCount = 0;
            int expectedResourcesCount = 0;
            Assert.That(expectedTasksCount == cloud.Tasks.Count);
            Assert.That(expectedResourcesCount == cloud.Resources.Count);
        }

        [Test]
        public void LogTaskShouldReturnTheCorrectString()
        {
            string expectedMsg = "Task logged successfully.";
            string[] args = new string[] { "2", "Java", "OOP" };
            Assert.That(expectedMsg, Is.EqualTo(cloud.LogTask(args)));
            Assert.IsTrue(cloud.Tasks.Any(t => t.ResourceName == "OOP"));
        }

        [Test]
        public void LogTaskShouldThrowIfNotExactly3Args()
        {
            string[] strings = new string[] { "JS", "NodeJS" };
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => cloud.LogTask(strings), "All arguments are required.");
        }

        [Test]
        public void LogTaskShouldThrowIfArgumentIsNull()
        {
            string[] strings = new string[] { "JS", "NodeJS", null };
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => cloud.LogTask(strings), "Arguments values cannot be null.");
        }

        [Test]
        public void LogTaskShouldReturnTheCorrectStringIfNameExists()
        {
            string[] args1 = new string[] { "2", "C#", "Inheritance" };
            string[] args2 = new string[] { "1", "Java", "Inheritance" };
            cloud.LogTask(args1);
            string expectedMsg = "Inheritance is already logged.";
            string actualMsg = cloud.LogTask(args2);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            int expectedCount = 1;
            Assert.IsTrue(cloud.Tasks.Count == expectedCount);
        }

        [Test]
        public void CreateResourceShouldReturnTrueIfOK()
        {
            string[] args1 = new string[] { "2", "C#", "Inheritance" };
            string[] args2 = new string[] { "1", "Java", "OOP" };
            cloud.LogTask(args1);
            cloud.LogTask(args2);
            Assert.IsTrue(cloud.CreateResource());
            Assert.IsFalse(cloud.Tasks.Any(t => t.ResourceName == "OOP"));
            Assert.IsTrue(cloud.Resources.Count == 1);
        }

        [Test]
        public void CreateResourceShouldReturnFalseIfNoTasks()
        {
            Assert.IsFalse(cloud.CreateResource());
            Assert.IsTrue(cloud.Tasks.Count == 0);
            Assert.IsTrue(cloud.Resources.Count == 0);
        }

        [Test]
        public void TestResourceShouldReturnNullOfResourceNotFound()
        {
            Assert.IsNull(cloud.TestResource("resource"));
        }

        [Test]
        public void TestResourceShouldReturnTheCorrectResource()
        {
            string[] args1 = new string[] { "2", "C#", "Inheritance" };
            string[] args2 = new string[] { "1", "Java", "OOP" };
            cloud.LogTask(args1);
            cloud.LogTask(args2);
            cloud.CreateResource();
            Resource resource = cloud.Resources.First();
            Assert.IsTrue(resource == cloud.TestResource("OOP"));
        }

        [Test]
        public void TestResourceShouldToggleIsTestedStatus()
        {
            string[] args1 = new string[] { "2", "C#", "Inheritance" };
            string[] args2 = new string[] { "1", "Java", "OOP" };
            cloud.LogTask(args1);
            cloud.LogTask(args2);
            cloud.CreateResource();
            Resource resource = cloud.Resources.First();
            cloud.TestResource("OOP");
            Assert.IsTrue(resource.IsTested);
        }
    }
}