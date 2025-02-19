﻿using NauticalCatchChallenge.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public abstract class Diver : IDiver
    {
        private string name;
        private int oxygenLevel;
        private double competitionPoints;
        private readonly List<string> _catch;

        protected Diver(string name, int oxygenLevel)
        {
            Name = name;
            OxygenLevel = oxygenLevel;
            _catch = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Diver's name cannot be null or empty.");
                }

                name = value;
            }
        }

        public int OxygenLevel
        {
            get => oxygenLevel;
            protected set
            {
                if (value < 0)
                {
                    value = 0;
                }

                oxygenLevel = value;
            }
        }

        public IReadOnlyCollection<string> Catch
            => _catch.AsReadOnly();

        public double CompetitionPoints
        {
            get => competitionPoints;
            private set
            {
                competitionPoints = Math.Round(value, 1);
            }
        }

        public bool HasHealthIssues { get; private set; }

        public void Hit(IFish fish)
        {
            OxygenLevel -= fish.TimeToCatch;
            _catch.Add(fish.Name);
            CompetitionPoints += fish.Points;
        }

        public abstract void Miss(int timeToCatch);

        public abstract void RenewOxy();

        public void UpdateHealthStatus()
        {
            HasHealthIssues = !HasHealthIssues;
        }

        public override string ToString()
        {
            return $"Diver [ Name: {Name}, Oxygen left: {OxygenLevel}, Fish caught: {Catch.Count}, Points earned: {CompetitionPoints} ]";
        }
    }
}
