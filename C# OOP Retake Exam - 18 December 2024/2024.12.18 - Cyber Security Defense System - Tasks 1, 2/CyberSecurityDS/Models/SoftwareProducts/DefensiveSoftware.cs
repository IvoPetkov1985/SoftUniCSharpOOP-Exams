using CyberSecurityDS.Models.Contracts;

namespace CyberSecurityDS.Models.SoftwareProducts
{
    public abstract class DefensiveSoftware : IDefensiveSoftware
    {
        private readonly List<string> assignedAttacks;
        private string name;
        private int effectiveness;

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

                if (value == 0)
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
            return $"Defensive Software: {Name}, Effectiveness: {Effectiveness}, Assigned Attacks: {(AssignedAttacks.Any() ? string.Join(", ", AssignedAttacks) : "[None]")}";
        }
    }
}
