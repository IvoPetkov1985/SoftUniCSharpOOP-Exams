using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Repositories
{
    public class DefensiveSoftwareRepository : IRepository<IDefensiveSoftware>
    {
        private readonly List<IDefensiveSoftware> softwareProducts;

        public DefensiveSoftwareRepository()
        {
            softwareProducts = new List<IDefensiveSoftware>();
        }

        public IReadOnlyCollection<IDefensiveSoftware> Models => softwareProducts.AsReadOnly();

        public void AddNew(IDefensiveSoftware software)
        {
            softwareProducts.Add(software);
        }

        public bool Exists(string name)
        {
            return softwareProducts.Any(s => s.Name == name);
        }

        public IDefensiveSoftware GetByName(string name)
        {
            IDefensiveSoftware software = softwareProducts.FirstOrDefault(s => s.Name == name);
            return software;
        }
    }
}
