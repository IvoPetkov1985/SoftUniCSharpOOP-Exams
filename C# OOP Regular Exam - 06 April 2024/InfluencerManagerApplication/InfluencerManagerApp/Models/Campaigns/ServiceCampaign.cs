namespace InfluencerManagerApp.Models.Campaigns
{
    public class ServiceCampaign : Campaign
    {
        private const double ServiceCampaignBudget = 30000;

        public ServiceCampaign(string brand)
            : base(brand, ServiceCampaignBudget)
        {
        }
    }
}
