namespace CyberSecurityDS.Models.CyberAttacks
{
    public class PhishingAttack : CyberAttack
    {
        private string targetMail;

        public PhishingAttack(string attackName, int severityLevel, string targetMail)
            : base(attackName, severityLevel)
        {
            TargetMail = targetMail;
        }

        public string TargetMail
        {
            get => targetMail;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Target mail is required.");
                }

                targetMail = value;
            }
        }

        public override string ToString()
        {
            return $"Attack: {AttackName}, Severity: {SeverityLevel} (Target Mail: {TargetMail})";
        }
    }
}
