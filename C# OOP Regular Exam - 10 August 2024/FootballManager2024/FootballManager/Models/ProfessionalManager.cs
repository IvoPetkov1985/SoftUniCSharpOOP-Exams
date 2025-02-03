namespace FootballManager.Models
{
    public class ProfessionalManager : Manager
    {
        private const double ProfessionalManagerRanking = 60.0;
        private const double ProfessionalManagerMultiplier = 1.5;

        public ProfessionalManager(string name)
            : base(name, ProfessionalManagerRanking)
        {
        }

        public override void RankingUpdate(double updateValue)
        {
            Ranking += updateValue * ProfessionalManagerMultiplier;

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
