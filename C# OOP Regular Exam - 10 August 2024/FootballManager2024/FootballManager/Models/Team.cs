using FootballManager.Models.Contracts;

namespace FootballManager.Models
{
    public class Team : ITeam
    {
        private string name;
        private IManager teamManager;

        public Team(string name)
        {
            Name = name;
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

        public int ChampionshipPoints { get; private set; }

        public IManager TeamManager
        {
            get => teamManager;
            private set
            {
                teamManager = value;
            }
        }

        public int PresentCondition
            => PresentConditionCalculator();

        public void GainPoints(int points)
        {
            ChampionshipPoints += points;
        }

        public void ResetPoints()
        {
            ChampionshipPoints = 0;
        }

        public void SignWith(IManager manager)
        {
            TeamManager = manager;
        }

        public override string ToString()
        {
            return $"Team: {Name} Points: {ChampionshipPoints}";
        }

        private int PresentConditionCalculator()
        {
            if (TeamManager == null)
            {
                return 0;
            }

            if (ChampionshipPoints == 0)
            {
                return (int)Math.Floor(TeamManager.Ranking);
            }

            return (int)Math.Floor(ChampionshipPoints * TeamManager.Ranking);
        }
    }
}
