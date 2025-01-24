namespace InfluencerManagerApp.Models.Influencers
{
    public class BloggerInfluencer : Influencer
    {
        private const double BloggerEngagementRate = 2.0;
        private const double BloggerInfluencerFactor = 0.2;

        public BloggerInfluencer(string username, int followers)
            : base(username, followers, BloggerEngagementRate)
        {
        }

        public override int CalculateCampaignPrice()
        {
            return (int)Math.Floor(Followers * EngagementRate * BloggerInfluencerFactor);
        }
    }
}
