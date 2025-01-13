using Handball.Models.Contracts;
using System;
using System.Text;

namespace Handball.Models.Players
{
    public abstract class Player : IPlayer
    {
        private string name;

        protected Player(string name, double rating)
        {
            Name = name;
            Rating = rating;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Player name cannot be null or empty.");
                }

                name = value;
            }
        }

        public double Rating { get; protected set; }

        public string Team { get; private set; }

        public abstract void DecreaseRating();

        public abstract void IncreaseRating();

        public void JoinTeam(string name)
        {
            Team = name;
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine($"{this.GetType().Name}: {Name}");
            builder.AppendLine($"--Rating: {Rating}");

            return builder.ToString().TrimEnd();
        }
    }
}
