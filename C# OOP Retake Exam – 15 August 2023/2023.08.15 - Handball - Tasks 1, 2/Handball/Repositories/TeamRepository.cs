using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Handball.Repositories
{
    public class TeamRepository : IRepository<ITeam>
    {
        private readonly List<ITeam> teams;

        public TeamRepository()
        {
            teams = new List<ITeam>();
        }

        public IReadOnlyCollection<ITeam> Models => teams.AsReadOnly();

        public void AddModel(ITeam team)
        {
            teams.Add(team);
        }

        public bool ExistsModel(string name)
        {
            ITeam team = teams.FirstOrDefault(t => t.Name == name);
            return team != null;
        }

        public ITeam GetModel(string name)
        {
            ITeam team = teams.FirstOrDefault(t => t.Name == name);
            return team;
        }

        public bool RemoveModel(string name)
        {
            ITeam team = teams.FirstOrDefault(t => t.Name == name);
            return teams.Remove(team);
        }
    }
}
