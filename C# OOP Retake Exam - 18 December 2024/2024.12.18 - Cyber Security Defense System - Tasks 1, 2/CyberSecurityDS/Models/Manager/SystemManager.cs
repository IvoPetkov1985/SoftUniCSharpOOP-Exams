using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Models.Manager
{
    public class SystemManager : ISystemManager
    {
        private readonly CyberAttackRepository cyberAttacks;
        private readonly DefensiveSoftwareRepository defensiveSoftwares;

        public SystemManager()
        {
            cyberAttacks = new CyberAttackRepository();
            defensiveSoftwares = new DefensiveSoftwareRepository();
        }

        public IRepository<ICyberAttack> CyberAttacks => cyberAttacks;

        public IRepository<IDefensiveSoftware> DefensiveSoftwares => defensiveSoftwares;
    }
}
