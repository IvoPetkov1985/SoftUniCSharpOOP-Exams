using FootballManager.Models.Contracts;

namespace FootballManager.Models
{
    public abstract class Manager : IManager
    {
        private string name;
        private double ranking;

        protected Manager(string name, double ranking)
        {
            Name = name;
            Ranking = ranking;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Manager's name cannot be null or empty.");
                }

                name = value;
            }
        }

        public double Ranking
        {
            get => ranking;
            protected set
            {
                ranking = value;
            }
        }

        public abstract void RankingUpdate(double updateValue);

        public override string ToString()
        {
            return $"{Name} - {this.GetType().Name} (Ranking: {Ranking:F2})";
        }
    }
}
