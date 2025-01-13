using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Handball.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        private readonly List<IPlayer> players;

        public PlayerRepository()
        {
            players = new List<IPlayer>();
        }

        public IReadOnlyCollection<IPlayer> Models => players.AsReadOnly();

        public void AddModel(IPlayer player)
        {
            players.Add(player);
        }

        public bool ExistsModel(string name)
        {
            IPlayer player = players.FirstOrDefault(p => p.Name == name);
            return player != null;
        }

        public IPlayer GetModel(string name)
        {
            IPlayer player = players.FirstOrDefault(p => p.Name == name);
            return player;
        }

        public bool RemoveModel(string name)
        {
            IPlayer player = players.FirstOrDefault(p => p.Name == name);
            return players.Remove(player);
        }
    }
}
