namespace FootballManager.Models
{
    public class AmateurManager : Manager
    {
        private const double AmateurManagerRanking = 15.0;
        private const double AmateurManagerMultiplier = 0.75;

        public AmateurManager(string name)
            : base(name, AmateurManagerRanking)
        {
        }

        public override void RankingUpdate(double updateValue)
        {
            Ranking += updateValue * AmateurManagerMultiplier;

            if (Ranking < 0)
            {
                Ranking = 0;
            }

            if (Ranking > 100)
            {
                Ranking = 100;
            }
        }
    }
}
