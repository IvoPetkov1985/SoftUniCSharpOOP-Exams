using InfluencerManagerApp.Models.Contracts;

namespace InfluencerManagerApp.Models.Influencers
{
    public abstract class Influencer : IInfluencer
    {
        private string username;
        private int followers;
        private readonly List<string> participations;

        protected Influencer(string username, int followers, double engagementRate)
        {
            Username = username;
            Followers = followers;
            EngagementRate = engagementRate;
            participations = new List<string>();
        }

        public string Username
        {
            get => username;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username is required.");
                }

                username = value;
            }
        }

        public int Followers
        {
            get => followers;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Followers count cannot be a negative number.");
                }

                followers = value;
            }
        }

        public double EngagementRate { get; private set; }

        public double Income { get; private set; }

        public IReadOnlyCollection<string> Participations
            => participations.AsReadOnly();

        public abstract int CalculateCampaignPrice();

        public void EarnFee(double amount)
        {
            Income += amount;
        }

        public void EndParticipation(string brand)
        {
            participations.Remove(brand);
        }

        public void EnrollCampaign(string brand)
        {
            participations.Add(brand);
        }

        public override string ToString()
        {
            return $"{Username} - Followers: {Followers}, Total Income: {Income}";
        }
    }
}
