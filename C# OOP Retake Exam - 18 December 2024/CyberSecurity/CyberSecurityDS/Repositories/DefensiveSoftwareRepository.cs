using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Repositories
{
    public class DefensiveSoftwareRepository : IRepository<IDefensiveSoftware>
    {
        private readonly List<IDefensiveSoftware> softwares;

        public DefensiveSoftwareRepository()
        {
            softwares = new List<IDefensiveSoftware>();
        }

        public IReadOnlyCollection<IDefensiveSoftware> Models
            => softwares.AsReadOnly();

        public void AddNew(IDefensiveSoftware model)
        {
            softwares.Add(model);
        }

        public bool Exists(string name)
        {
            return softwares.Any(s => s.Name == name);
        }

        public IDefensiveSoftware GetByName(string name)
        {
            return softwares.FirstOrDefault(s => s.Name == name);
        }
    }
}
