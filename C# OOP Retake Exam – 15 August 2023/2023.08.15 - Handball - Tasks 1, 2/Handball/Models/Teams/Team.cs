using Handball.Models.Contracts;
using Handball.Models.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handball.Models.Teams
{
    public class Team : ITeam
    {
        private string name;
        private readonly List<IPlayer> players;

        public Team(string name)
        {
            Name = name;
            players = new List<IPlayer>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Team name cannot be null or empty.");
                }

                name = value;
            }
        }

        public int PointsEarned { get; private set; }

        public double OverallRating
        {
            get
            {
                if (players.Count == 0)
                {
                    return 0;
                }

                return Math.Round(players.Average(p => p.Rating), 2);
            }
        }

        public IReadOnlyCollection<IPlayer> Players => players.AsReadOnly();

        public void Draw()
        {
            PointsEarned += 1;

            IPlayer goalkeeper = players.First(p => p.GetType().Name == nameof(Goalkeeper));
            goalkeeper.IncreaseRating();
        }

        public void Lose()
        {
            foreach (IPlayer player in players)
            {
                player.DecreaseRating();
            }
        }

        public void SignContract(IPlayer player)
        {
            players.Add(player);
        }

        public void Win()
        {
            PointsEarned += 3;

            foreach (IPlayer player in players)
            {
                player.IncreaseRating();
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine($"Team: {Name} Points: {PointsEarned}");
            builder.AppendLine($"--Overall rating: {OverallRating}");
            builder.AppendLine($"--Players: {(players.Any() ? string.Join(", ", players.Select(p => p.Name)) : "none")}");

            return builder.ToString().TrimEnd();
        }
    }
}
