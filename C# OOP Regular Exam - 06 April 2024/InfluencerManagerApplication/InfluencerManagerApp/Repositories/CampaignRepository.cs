using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Repositories.Contracts;

namespace InfluencerManagerApp.Repositories
{
    public class CampaignRepository : IRepository<ICampaign>
    {
        private readonly List<ICampaign> campaigns;

        public CampaignRepository()
        {
            campaigns = new List<ICampaign>();
        }

        public IReadOnlyCollection<ICampaign> Models
            => campaigns.AsReadOnly();

        public void AddModel(ICampaign model)
        {
            campaigns.Add(model);
        }

        public ICampaign FindByName(string name)
        {
            return campaigns.FirstOrDefault(c => c.Brand == name);
        }

        public bool RemoveModel(ICampaign model)
        {
            return campaigns.Remove(model);
        }
    }
}
