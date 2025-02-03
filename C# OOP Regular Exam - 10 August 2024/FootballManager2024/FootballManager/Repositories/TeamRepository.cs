using FootballManager.Models.Contracts;
using FootballManager.Repositories.Contracts;

namespace FootballManager.Repositories
{
    public class TeamRepository : IRepository<ITeam>
    {
        private const int MaxCapacity = 10;
        private readonly List<ITeam> teams;

        public TeamRepository()
        {
            teams = new List<ITeam>();
        }

        public IReadOnlyCollection<ITeam> Models
            => teams.AsReadOnly();

        public int Capacity
            => MaxCapacity;

        public void Add(ITeam model)
        {
            if (teams.Count < Capacity)
            {
                teams.Add(model);
            }
        }

        public bool Exists(string name)
        {
            return teams.Any(t => t.Name == name);
        }

        public ITeam Get(string name)
        {
            return teams.FirstOrDefault(t => t.Name == name);
        }

        public bool Remove(string name)
        {
            ITeam teamToRemove = teams.FirstOrDefault(t => t.Name == name);
            return teams.Remove(teamToRemove);
        }
    }
}
