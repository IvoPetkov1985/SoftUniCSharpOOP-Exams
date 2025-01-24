namespace InfluencerManagerApp.Models.Influencers
{
    public class BusinessInfluencer : Influencer
    {
        private const double BusinessEngagementRate = 3.0;
        private const double BusinessInfluencerFactor = 0.15;

        public BusinessInfluencer(string username, int followers)
            : base(username, followers, BusinessEngagementRate)
        {
        }

        public override int CalculateCampaignPrice()
        {
            return (int)Math.Floor(Followers * EngagementRate * BusinessInfluencerFactor);
        }
    }
}
