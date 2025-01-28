using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Models.SystemManagers
{
    public class SystemManager : ISystemManager
    {
        private readonly IRepository<ICyberAttack> cyberAttacks;
        private readonly IRepository<IDefensiveSoftware> defensiveSoftwares;

        public SystemManager()
        {
            cyberAttacks = new CyberAttackRepository();
            defensiveSoftwares = new DefensiveSoftwareRepository();
        }

        public IRepository<ICyberAttack> CyberAttacks
            => cyberAttacks;

        public IRepository<IDefensiveSoftware> DefensiveSoftwares
            => defensiveSoftwares;
    }
}
