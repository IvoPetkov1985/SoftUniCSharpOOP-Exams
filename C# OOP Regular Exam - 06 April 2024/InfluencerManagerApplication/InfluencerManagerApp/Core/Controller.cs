using InfluencerManagerApp.Core.Contracts;
using InfluencerManagerApp.Models.Campaigns;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Models.Influencers;
using InfluencerManagerApp.Repositories;
using InfluencerManagerApp.Repositories.Contracts;
using System.Text;

namespace InfluencerManagerApp.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IInfluencer> influencers;
        private readonly IRepository<ICampaign> campaigns;

        public Controller()
        {
            influencers = new InfluencerRepository();
            campaigns = new CampaignRepository();
        }

        public string ApplicationReport()
        {
            IEnumerable<IInfluencer> orderedInfluencers = influencers.Models
                .OrderByDescending(i => i.Income)
                .ThenByDescending(i => i.Followers);

            StringBuilder builder = new();

            foreach (IInfluencer influencer in orderedInfluencers)
            {
                builder.AppendLine(influencer.ToString());

                if (influencer.Participations.Any())
                {
                    builder.AppendLine("Active Campaigns:");

                    foreach (string brand in influencer.Participations)
                    {
                        ICampaign campaign = campaigns.FindByName(brand);
                        builder.AppendLine($"--{campaign.ToString()}");
                    }
                }
            }

            return builder.ToString().TrimEnd();
        }

        public string AttractInfluencer(string brand, string username)
        {
            IInfluencer influencer = influencers.FindByName(username);

            if (influencer == null)
            {
                return $"{influencers.GetType().Name} has no {username} registered in the application.";
            }

            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign == null)
            {
                return $"There is no campaign from {brand} in the application.";
            }

            if (campaign.Contributors.Contains(username))
            {
                return $"{username} is already engaged for the {brand} campaign.";
            }

            if (campaign.GetType().Name == nameof(ProductCampaign) && influencer.GetType().Name == nameof(BloggerInfluencer))
            {
                return $"{username} is not eligible for the {brand} campaign.";
            }

            if (campaign.GetType().Name == nameof(ServiceCampaign) && influencer.GetType().Name == nameof(FashionInfluencer))
            {
                return $"{username} is not eligible for the {brand} campaign.";
            }

            if (campaign.Budget < influencer.CalculateCampaignPrice())
            {
                return $"The budget for {brand} is insufficient to engage {username}.";
            }

            influencer.EarnFee(influencer.CalculateCampaignPrice());
            influencer.EnrollCampaign(brand);
            campaign.Engage(influencer);
            return $"{username} has been successfully attracted to the {brand} campaign.";
        }

        public string BeginCampaign(string typeName, string brand)
        {
            if (typeName != nameof(ServiceCampaign) &&
                typeName != nameof(ProductCampaign))
            {
                return $"{typeName} is not a valid campaign in the application.";
            }

            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign != null)
            {
                return $"{brand} campaign cannot be duplicated.";
            }

            if (typeName == nameof(ServiceCampaign))
            {
                campaign = new ServiceCampaign(brand);
            }
            else
            {
                campaign = new ProductCampaign(brand);
            }

            campaigns.AddModel(campaign);
            return $"{brand} started a {typeName}.";
        }

        public string CloseCampaign(string brand)
        {
            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign == null)
            {
                return "Trying to close an invalid campaign.";
            }

            if (campaign.Budget <= 10000)
            {
                return $"{brand} campaign cannot be closed as it has not met its financial targets.";
            }

            foreach (string username in campaign.Contributors)
            {
                IInfluencer influencer = influencers.FindByName(username);
                influencer.EarnFee(2000);
                influencer.EndParticipation(brand);
            }

            campaigns.RemoveModel(campaign);
            return $"{brand} campaign has reached its target.";
        }

        public string ConcludeAppContract(string username)
        {
            IInfluencer influencer = influencers.FindByName(username);

            if (influencer == null)
            {
                return $"{username} has still not signed a contract.";
            }

            if (influencer.Participations.Any())
            {
                return $"{username} cannot conclude the contract while enrolled in active campaigns.";
            }

            influencers.RemoveModel(influencer);
            return $"{username} concluded their contract.";
        }

        public string FundCampaign(string brand, double amount)
        {
            ICampaign campaign = campaigns.FindByName(brand);

            if (campaign == null)
            {
                return "Trying to fund an invalid campaign.";
            }

            if (amount <= 0)
            {
                return "Funding amount must be greater than zero.";
            }

            campaign.Gain(amount);
            return $"{brand} campaign has been successfully funded with {amount} $";
        }

        public string RegisterInfluencer(string typeName, string username, int followers)
        {
            if (typeName != nameof(BusinessInfluencer) &&
                typeName != nameof(FashionInfluencer) &&
                typeName != nameof(BloggerInfluencer))
            {
                return $"{typeName} has not passed validation.";
            }

            IInfluencer influencer = influencers.FindByName(username);

            if (influencer != null)
            {
                return $"{username} is already registered in {influencers.GetType().Name}.";
            }

            if (typeName == nameof(BusinessInfluencer))
            {
                influencer = new BusinessInfluencer(username, followers);
            }
            else if (typeName == nameof(FashionInfluencer))
            {
                influencer = new FashionInfluencer(username, followers);
            }
            else
            {
                influencer = new BloggerInfluencer(username, followers);
            }

            influencers.AddModel(influencer);
            return $"{username} registered successfully to the application.";
        }
    }
}
