namespace InfluencerManagerApp.Models.Influencers
{
    public class FashionInfluencer : Influencer
    {
        private const double FashionEngagementRate = 4.0;
        private const double FashionInfluencerFactor = 0.1;

        public FashionInfluencer(string username, int followers)
            : base(username, followers, FashionEngagementRate)
        {
        }

        public override int CalculateCampaignPrice()
        {
            return (int)Math.Floor(Followers * EngagementRate * FashionInfluencerFactor);
        }
    }
}
