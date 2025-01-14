using CyberSecurityDS.Core.Contracts;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Models.CyberAttacks;
using CyberSecurityDS.Models.Manager;
using CyberSecurityDS.Models.SoftwareProducts;
using System.Text;

namespace CyberSecurityDS.Core
{
    public class Controller : IController
    {
        private readonly ISystemManager manager;

        public Controller()
        {
            manager = new SystemManager();
        }

        public string AddCyberAttack(string attackType, string attackName, int severityLevel, string extraParam)
        {
            if (attackType != nameof(PhishingAttack) && attackType != nameof(MalwareAttack))
            {
                return $"{attackType} is not a valid type for the system.";
            }

            if (manager.CyberAttacks.Exists(attackName))
            {
                return $"{attackName} already exists in the system.";
            }

            ICyberAttack attack = null;

            if (attackType == nameof(PhishingAttack))
            {
                attack = new PhishingAttack(attackName, severityLevel, extraParam);
            }
            else
            {
                attack = new MalwareAttack(attackName, severityLevel, extraParam);
            }

            manager.CyberAttacks.AddNew(attack);
            return $"{attackType}: {attackName} is added to the system.";
        }

        public string AddDefensiveSoftware(string softwareType, string softwareName, int effectiveness)
        {
            if (softwareType != nameof(Firewall) && softwareType != nameof(Antivirus))
            {
                return $"{softwareType} is not a valid type for the system.";
            }

            if (manager.DefensiveSoftwares.Exists(softwareName))
            {
                return $"{softwareName} already exists in the system.";
            }

            IDefensiveSoftware software = null;

            if (softwareType == nameof(Firewall))
            {
                software = new Firewall(softwareName, effectiveness);
            }
            else
            {
                software = new Antivirus(softwareName, effectiveness);
            }

            manager.DefensiveSoftwares.AddNew(software);
            return $"{softwareType}: {softwareName} is added to the system.";
        }

        public string AssignDefense(string cyberAttackName, string defensiveSoftwareName)
        {
            if (manager.CyberAttacks.Exists(cyberAttackName) == false)
            {
                return $"{cyberAttackName} does not exist in the system.";
            }

            if (manager.DefensiveSoftwares.Exists(defensiveSoftwareName) == false)
            {
                return $"{defensiveSoftwareName} does not exist in the system.";
            }

            ICyberAttack attack = manager.CyberAttacks.GetByName(cyberAttackName);
            IDefensiveSoftware software = manager.DefensiveSoftwares.GetByName(defensiveSoftwareName);

            if (manager.DefensiveSoftwares.Models.Any(m => m.AssignedAttacks.Contains(cyberAttackName)))
            {
                IDefensiveSoftware assigned = manager.DefensiveSoftwares.Models
                    .First(s => s.AssignedAttacks.Contains(cyberAttackName));

                return $"{cyberAttackName} is already assigned to {assigned.Name}.";
            }

            software.AssignAttack(cyberAttackName);
            return $"{cyberAttackName} is assigned to {defensiveSoftwareName}.";
        }

        public string GenerateReport()
        {
            StringBuilder builder = new();

            builder.AppendLine("Security:");

            foreach (IDefensiveSoftware software in manager.DefensiveSoftwares.Models.OrderBy(s => s.Name))
            {
                builder.AppendLine(software.ToString());
            }

            builder.AppendLine("Threads:");
            builder.AppendLine("-Mitigated:");

            IEnumerable<ICyberAttack> completed = manager.CyberAttacks.Models
                .Where(a => a.Status == true).OrderBy(a => a.AttackName);
            IEnumerable<ICyberAttack> pending = manager.CyberAttacks.Models
                .Where(a => a.Status == false).OrderBy(a => a.AttackName);

            foreach (ICyberAttack attack in completed)
            {
                builder.AppendLine(attack.ToString());
            }

            builder.AppendLine("-Pending:");

            foreach (ICyberAttack attack1 in pending)
            {
                builder.AppendLine(attack1.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string MitigateAttack(string cyberAttackName)
        {
            if (manager.CyberAttacks.Exists(cyberAttackName) == false)
            {
                return $"{cyberAttackName} does not exist in the system.";
            }

            ICyberAttack attack = manager.CyberAttacks.GetByName(cyberAttackName);

            if (attack.Status == true)
            {
                return $"{cyberAttackName} is already mitigated.";
            }

            if (manager.DefensiveSoftwares.Models.Any(s => s.AssignedAttacks.Contains(cyberAttackName)) == false)
            {
                return $"{cyberAttackName} is not assigned yet.";
            }

            IDefensiveSoftware software = manager.DefensiveSoftwares.Models
                .First(s => s.AssignedAttacks.Contains(cyberAttackName));

            if ((software.GetType().Name == nameof(Antivirus) && attack.GetType().Name == nameof(MalwareAttack)) ||
                (software.GetType().Name == nameof(Firewall) && attack.GetType().Name == nameof(PhishingAttack)))
            {
                return $"{software.GetType().Name} cannot mitigate {attack.GetType().Name}.";
            }

            int softwareEffectiveness = software.Effectiveness;
            int attackSeverity = attack.SeverityLevel;

            if (softwareEffectiveness >= attackSeverity)
            {
                attack.MarkAsMitigated();
                return $"{cyberAttackName} is mitigated successfully.";
            }
            else
            {
                return $"{cyberAttackName} could not be mitigated by {software.Name}.";
            }
        }
    }
}
