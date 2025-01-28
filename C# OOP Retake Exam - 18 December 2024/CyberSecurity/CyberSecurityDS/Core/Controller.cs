using CyberSecurityDS.Core.Contracts;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Models.CyberAttacks;
using CyberSecurityDS.Models.DefensiveSoftwares;
using CyberSecurityDS.Models.SystemManagers;
using System.Text;

namespace CyberSecurityDS.Core
{
    public class Controller : IController
    {
        private readonly ISystemManager systemManager;

        public Controller()
        {
            systemManager = new SystemManager();
        }

        public string AddCyberAttack(string attackType, string attackName, int severityLevel, string extraParam)
        {
            if (attackType != nameof(MalwareAttack) &&
                attackType != nameof(PhishingAttack))
            {
                return $"{attackType} is not a valid type for the system.";
            }

            if (systemManager.CyberAttacks.Exists(attackName))
            {
                return $"{attackName} already exists in the system.";
            }

            ICyberAttack attack = null;

            if (attackType == nameof(MalwareAttack))
            {
                attack = new MalwareAttack(attackName, severityLevel, extraParam);
            }
            else
            {
                attack = new PhishingAttack(attackName, severityLevel, extraParam);
            }

            systemManager.CyberAttacks.AddNew(attack);
            return $"{attackType}: {attackName} is added to the system.";
        }

        public string AddDefensiveSoftware(string softwareType, string softwareName, int effectiveness)
        {
            if (softwareType != nameof(Firewall) &&
                softwareType != nameof(Antivirus))
            {
                return $"{softwareType} is not a valid type for the system.";
            }

            if (systemManager.DefensiveSoftwares.Exists(softwareName))
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

            systemManager.DefensiveSoftwares.AddNew(software);
            return $"{softwareType}: {softwareName} is added to the system.";
        }

        public string AssignDefense(string cyberAttackName, string defensiveSoftwareName)
        {
            ICyberAttack attack = systemManager.CyberAttacks.GetByName(cyberAttackName);

            if (attack == null)
            {
                return $"{cyberAttackName} does not exist in the system.";
            }

            IDefensiveSoftware software = systemManager.DefensiveSoftwares.GetByName(defensiveSoftwareName);

            if (software == null)
            {
                return $"{defensiveSoftwareName} does not exist in the system.";
            }

            foreach (IDefensiveSoftware anySoftware in systemManager.DefensiveSoftwares.Models)
            {
                if (anySoftware.AssignedAttacks.Contains(cyberAttackName))
                {
                    return $"{cyberAttackName} is already assigned to {anySoftware.Name}.";
                }
            }

            software.AssignAttack(cyberAttackName);
            return $"{cyberAttackName} is assigned to {defensiveSoftwareName}.";
        }

        public string GenerateReport()
        {
            StringBuilder builder = new();
            builder.AppendLine("Security:");

            foreach (IDefensiveSoftware software in systemManager.DefensiveSoftwares.Models
                .OrderBy(df => df.Name))
            {
                builder.AppendLine(software.ToString());
            }

            IEnumerable<ICyberAttack> mitigatedAttacks = systemManager.CyberAttacks.Models
                .Where(ca => ca.Status == true)
                .OrderBy(ca => ca.AttackName);

            builder.AppendLine("Threads:");

            if (mitigatedAttacks.Any())
            {
                builder.AppendLine("-Mitigated:");

                foreach (ICyberAttack attack in mitigatedAttacks)
                {
                    builder.AppendLine(attack.ToString());
                }
            }

            IEnumerable<ICyberAttack> pendingAttacks = systemManager.CyberAttacks.Models
                .Where(ca => ca.Status == false)
                .OrderBy(ca => ca.AttackName);

            if (pendingAttacks.Any())
            {
                builder.AppendLine("-Pending:");

                foreach (ICyberAttack attack in pendingAttacks)
                {
                    builder.AppendLine(attack.ToString());
                }
            }

            return builder.ToString().TrimEnd();
        }

        public string MitigateAttack(string cyberAttackName)
        {
            ICyberAttack attack = systemManager.CyberAttacks.GetByName(cyberAttackName);

            if (attack == null)
            {
                return $"{cyberAttackName} does not exist in the system.";
            }

            if (attack.Status == true)
            {
                return $"{cyberAttackName} is already mitigated.";
            }

            if (!systemManager.DefensiveSoftwares.Models.Any(df => df.AssignedAttacks.Contains(cyberAttackName)))
            {
                return $"{cyberAttackName} is not assigned yet.";
            }

            IDefensiveSoftware software = systemManager.DefensiveSoftwares.Models
                .Where(df => df.AssignedAttacks.Contains(cyberAttackName))
                .First();

            if ((software.GetType().Name == nameof(Firewall) &&
                attack.GetType().Name == nameof(PhishingAttack)) ||
                (software.GetType().Name == nameof(Antivirus) &&
                attack.GetType().Name == nameof(MalwareAttack)))
            {
                return $"{software.GetType().Name} cannot mitigate {attack.GetType().Name}.";
            }

            if (software.Effectiveness >= attack.SeverityLevel)
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
