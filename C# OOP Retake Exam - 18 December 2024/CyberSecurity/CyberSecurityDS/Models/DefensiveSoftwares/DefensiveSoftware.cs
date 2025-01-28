using CyberSecurityDS.Models.Contracts;

namespace CyberSecurityDS.Models.DefensiveSoftwares
{
    public abstract class DefensiveSoftware : IDefensiveSoftware
    {
        private string name;
        private int effectiveness;
        private readonly List<string> assignedAttacks;

        protected DefensiveSoftware(string name, int effectiveness)
        {
            Name = name;
            Effectiveness = effectiveness;
            assignedAttacks = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Software name is required.");
                }

                name = value;
            }
        }

        public int Effectiveness
        {
            get => effectiveness;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Effectiveness cannot assign negative values.");
                }

                if (value < 1)
                {
                    value = 1;
                }

                if (value > 10)
                {
                    value = 10;
                }

                effectiveness = value;
            }
        }

        public IReadOnlyCollection<string> AssignedAttacks
            => assignedAttacks.AsReadOnly();

        public void AssignAttack(string attackName)
        {
            assignedAttacks.Add(attackName);
        }

        public override string ToString()
        {
            string attacksStatus = AssignedAttacks.Any() ? string.Join(", ", AssignedAttacks) : "[None]";
            return $"Defensive Software: {Name}, Effectiveness: {Effectiveness}, Assigned Attacks: {attacksStatus}";
        }
    }
}
