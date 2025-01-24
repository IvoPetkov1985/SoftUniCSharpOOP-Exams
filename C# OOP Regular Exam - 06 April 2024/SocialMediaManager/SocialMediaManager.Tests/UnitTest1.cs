using System;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace SocialMediaManager.Tests
{
    public class Tests
    {
        private InfluencerRepository repo;
        private Influencer influencer1;
        private Influencer influencer2;
        private Influencer influencer3;

        [SetUp]
        public void Setup()
        {
            repo = new();
            influencer1 = new("Dimitrichko", 50);
            influencer2 = new("Goshko", 10);
            influencer3 = new("Penka", 20);
        }

        [Test]
        public void RepoConstructorShouldInitializeProperly()
        {
            InfluencerRepository repo = new();
            Assert.IsNotNull(repo);
            Assert.IsNotNull(repo.Influencers);
        }

        [Test]
        public void InfluencersCountInitiallyShouldBeSetToZero()
        {
            int expectedCount = 0;
            Assert.That(expectedCount == repo.Influencers.Count);
        }

        [Test]
        public void RegisterInfluencerShouldReturnTheCorrectMsg()
        {
            string expectedMsg = "Successfully added influencer Dimitrichko with 50";
            string actualMsg = repo.RegisterInfluencer(influencer1);
            Assert.That(expectedMsg == actualMsg);
            Assert.That(repo.Influencers.Any(i => i.Username == "Dimitrichko"));
        }

        [Test]
        public void RegisterUserShouldThrowIfUsernameExists()
        {
            repo.RegisterInfluencer(influencer2);
            Influencer influencer4 = new("Goshko", 150);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => repo.RegisterInfluencer(influencer4), "Influencer with username Goshko already exists");
            Assert.IsFalse(repo.Influencers.Any(i => i.Followers == 150));
        }

        [Test]
        public void RegisterUserShouldThrowIfInfluencerIsNull()
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer2);
            repo.RegisterInfluencer(influencer3);
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => repo.RegisterInfluencer(null));
        }

        [Test]
        public void RemoveInfluencerShouldReturnTrueIfOK()
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer2);
            repo.RegisterInfluencer(influencer3);
            Assert.IsTrue(repo.RemoveInfluencer("Dimitrichko"));
            int expectedCount = 2;
            Assert.That(expectedCount == repo.Influencers.Count);
            Assert.IsFalse(repo.Influencers.Contains(influencer1));
        }

        [Test]
        public void RemoveInfluencerShouldReturnFalseIfUsernameNotExisting()
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer2);
            repo.RegisterInfluencer(influencer3);
            Assert.IsFalse(repo.RemoveInfluencer("Petranka"));
            int expectedCount = 3;
            Assert.That(expectedCount == repo.Influencers.Count);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        public void RemoveInfluencerShouldThrowIfNameIsNullOrWhiteSpace(string username)
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer3);
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => repo.RemoveInfluencer(username));
        }

        [Test]
        public void GetInfluencerWithMostFollowersShouldReturnTheCorrectResult()
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer2);
            repo.RegisterInfluencer(influencer3);
            Influencer actual = repo.GetInfluencerWithMostFollowers();
            Assert.That(influencer1, Is.EqualTo(actual));
        }

        [Test]
        public void GetInfluencerShouldReturnTheCorrectResult()
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer2);
            repo.RegisterInfluencer(influencer3);
            Influencer actual = repo.GetInfluencer("Dimitrichko");
            Assert.That(influencer1, Is.EqualTo(actual));
        }

        [Test]
        public void GetInfluencerShouldReturnNullIfNonExisting()
        {
            repo.RegisterInfluencer(influencer1);
            repo.RegisterInfluencer(influencer2);
            repo.RegisterInfluencer(influencer3);
            Assert.IsNull(repo.GetInfluencer("Ivancho"));
        }
    }
}
