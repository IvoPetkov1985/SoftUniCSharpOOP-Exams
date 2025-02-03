namespace FootballManager.Models
{
    public class SeniorManager : Manager
    {
        private const double SeniorManagerRanking = 30.0;

        public SeniorManager(string name)
            : base(name, SeniorManagerRanking)
        {
        }

        public override void RankingUpdate(double updateValue)
        {
            Ranking += updateValue;

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
