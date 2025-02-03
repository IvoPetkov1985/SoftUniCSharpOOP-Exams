using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Championship.Tests
{
    public class Tests
    {
        private League league1;
        private Team team1;
        private Team team2;
        private Team team3;
        private Team team4;
        private Team team5;
        private Team team6;
        private Team team7;
        private Team team8;
        private Team team9;
        private Team team10;

        [SetUp]
        public void Setup()
        {
            league1 = new League();
            team1 = new("PSG");
            team2 = new("Marseille");
            team3 = new("Saint Etienne");
            team4 = new("Lens");
            team5 = new("Angers");
            team6 = new("Monaco");
            team7 = new("Lyon");
            team8 = new("Brest");
            team9 = new("Rennais");
            team10 = new("Toulouse");
        }

        [Test]
        public void LeagueConstructorShouldInitializeCorrectly()
        {
            League serieA = new League();
            int expectedCapacity = 10;
            int actualCapacity = serieA.Capacity;
            Assert.IsNotNull(serieA);
            Assert.IsNotNull(serieA.Teams);
            Assert.That(expectedCapacity, Is.EqualTo(actualCapacity));
        }

        [Test]
        public void TeamsCountInitiallyShouldBeSetToZero()
        {
            int expectedCount = 0;
            int actualCount = league1.Teams.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void AddTeamMethodShouldWorkCorrectly()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            int expectedCount = 3;
            int actualCount = league1.Teams.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
            Assert.IsTrue(league1.Teams.Any(t => t.Name == "PSG"));
        }

        [Test]
        public void AddTeamMethodShouldThrowIfTeamWithTheSameNameAdded()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            Team team11 = new("Marseille");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => league1.AddTeam(team11), "Team already exists.");
        }

        [Test]
        public void AddTeamMethodShouldThrowIfCapacityExceeded()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            league1.AddTeam(team5);
            league1.AddTeam(team6);
            league1.AddTeam(team7);
            league1.AddTeam(team8);
            league1.AddTeam(team9);
            league1.AddTeam(team10);
            Team team11 = new("Nice");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => league1.AddTeam(team11), "League is full.");
            Assert.IsFalse(league1.Teams.Contains(team11));
            int expectedCount = 10;
            int actualCount = league1.Teams.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void RemoveTeamShouldReturnFalseIfTeamDoesNotExist()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            Assert.IsFalse(league1.RemoveTeam("Reims"));
        }

        [Test]
        public void RemoveTeamShouldReturnTrueIfDeletionIsSuccessful()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            Assert.IsTrue(league1.RemoveTeam("Marseille"));
            Assert.IsTrue(league1.RemoveTeam("PSG"));
            int expectedCount = 2;
            int actualCount = league1.Teams.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
            Assert.IsFalse(league1.Teams.Contains(team1));
            Assert.IsFalse(league1.Teams.Contains(team2));
        }

        [Test]
        public void PlayMatchShouldThrowIfATeamIsNull()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            league1.AddTeam(team5);
            league1.AddTeam(team6);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => league1.PlayMatch("PSG", "Antibes", 3, 1), "One or both teams do not exist.");

            InvalidOperationException ex1 = Assert.Throws<InvalidOperationException>(()
                => league1.PlayMatch("Auxerre", "Antibes", 3, 1), "One or both teams do not exist.");

            InvalidOperationException ex2 = Assert.Throws<InvalidOperationException>(()
                => league1.PlayMatch("Paris FC", "Lens", 3, 1), "One or both teams do not exist.");
        }

        [Test]
        public void PlayMatchShouldScoreCorrectlyIfDraw()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            league1.PlayMatch("Lens", "Marseille", 2, 2);
            league1.PlayMatch("Saint Etienne", "PSG", 1, 1);
            int expectedDrawsCount = 1;
            int expectedPointsCount = 1;
            Assert.That(expectedDrawsCount, Is.EqualTo(team1.Draws));
            Assert.That(expectedDrawsCount, Is.EqualTo(team2.Draws));
            Assert.That(expectedDrawsCount, Is.EqualTo(team3.Draws));
            Assert.That(expectedDrawsCount, Is.EqualTo(team4.Draws));
            Assert.That(expectedPointsCount, Is.EqualTo(team4.Points));
            Assert.That(expectedPointsCount, Is.EqualTo(team4.Points));
            Assert.That(expectedPointsCount, Is.EqualTo(team4.Points));
            Assert.That(expectedPointsCount, Is.EqualTo(team4.Points));
        }

        [Test]
        public void PlayMatchShouldScoreCorrectlyIfHomeTeamWins()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            league1.PlayMatch("PSG", "Marseille", 4, 0);
            league1.PlayMatch("PSG", "Lens", 2, 1);
            int expectedPSGPoints = 6;
            int expectedLensPoints = 0;
            int expectedPSGWinsCount = 2;
            int expectedLensLensLosesCount = 1;
            Assert.That(expectedPSGPoints, Is.EqualTo(team1.Points));
            Assert.That(expectedPSGWinsCount, Is.EqualTo(team1.Wins));
            Assert.That(expectedLensPoints, Is.EqualTo(team4.Points));
            Assert.That(expectedLensLensLosesCount, Is.EqualTo(team4.Loses));
        }

        [Test]
        public void PlayMatchShouldScoreCorrectlyIfGuestTeamIsTheWinner()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team2);
            league1.AddTeam(team3);
            league1.AddTeam(team4);
            league1.PlayMatch("Marseille", "Lens", 1, 2);
            league1.PlayMatch("Lens", "Marseille", 0, 3);
            int expectedWinsCount = 1;
            int expectedLosesCount = 1;
            int expectedPoints = 3;
            Assert.That(expectedWinsCount, Is.EqualTo(team2.Wins));
            Assert.That(expectedLosesCount, Is.EqualTo(team2.Loses));
            Assert.That(expectedWinsCount, Is.EqualTo(team4.Wins));
            Assert.That(expectedLosesCount, Is.EqualTo(team4.Loses));
            Assert.That(expectedPoints, Is.EqualTo(team2.Points));
            Assert.That(expectedPoints, Is.EqualTo(team4.Points));
        }

        [Test]
        public void GetTeamInfoShouldThrowIfTeamDoesNotExist()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team4);
            league1.AddTeam(team5);
            league1.AddTeam(team6);
            league1.AddTeam(team7);
            league1.AddTeam(team10);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => league1.GetTeamInfo("Montpellier"), "Team does not exist.");
        }

        [Test]
        public void GetTeamInfoShouldReturnTheCorrectString()
        {
            league1.AddTeam(team1);
            league1.AddTeam(team4);
            league1.AddTeam(team5);
            league1.AddTeam(team6);
            league1.AddTeam(team7);
            league1.AddTeam(team10);
            league1.PlayMatch("PSG", "Lens", 1, 1);
            league1.PlayMatch("PSG", "Monaco", 3, 4);
            league1.PlayMatch("PSG", "Lyon", 3, 1);
            string expectedMsg = "PSG - 4 points (1W 1D 1L)";
            string actualMsg = league1.GetTeamInfo("PSG");
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }
    }
}
