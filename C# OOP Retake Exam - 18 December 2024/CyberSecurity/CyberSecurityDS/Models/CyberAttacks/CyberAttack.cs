using CyberSecurityDS.Models.Contracts;

namespace CyberSecurityDS.Models.CyberAttacks
{
    public abstract class CyberAttack : ICyberAttack
    {
        private string attackName;
        private int severityLevel;

        protected CyberAttack(string attackName, int severityLevel)
        {
            AttackName = attackName;
            SeverityLevel = severityLevel;
            Status = false;
        }

        public string AttackName
        {
            get => attackName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Attack name is required.");
                }

                attackName = value;
            }
        }

        public int SeverityLevel
        {
            get => severityLevel;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Severity level cannot assign negative values.");
                }

                if (value < 1)
                {
                    value = 1;
                }

                if (value > 10)
                {
                    value = 10;
                }

                severityLevel = value;
            }
        }

        public bool Status { get; private set; }

        public void MarkAsMitigated()
        {
            Status = true;
        }
    }
}
