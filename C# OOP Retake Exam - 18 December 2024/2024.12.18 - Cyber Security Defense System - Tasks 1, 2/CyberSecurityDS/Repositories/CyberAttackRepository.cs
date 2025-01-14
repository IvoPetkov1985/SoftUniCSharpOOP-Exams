using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Repositories
{
    public class CyberAttackRepository : IRepository<ICyberAttack>
    {
        private readonly List<ICyberAttack> attacks;

        public CyberAttackRepository()
        {
            attacks = new List<ICyberAttack>();
        }

        public IReadOnlyCollection<ICyberAttack> Models => attacks.AsReadOnly();

        public void AddNew(ICyberAttack attack)
        {
            attacks.Add(attack);
        }

        public bool Exists(string name)
        {
            return attacks.Any(a => a.AttackName == name);
        }

        public ICyberAttack GetByName(string name)
        {
            ICyberAttack attack = attacks.FirstOrDefault(a => a.AttackName == name);
            return attack;
        }
    }
}
