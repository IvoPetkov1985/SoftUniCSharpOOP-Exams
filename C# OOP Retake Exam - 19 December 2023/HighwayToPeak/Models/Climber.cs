using HighwayToPeak.Models.Contracts;
using System.Text;

namespace HighwayToPeak.Models
{
    public abstract class Climber : IClimber
    {
        private string name;
        private int stamina;
        private readonly List<string> conqueredPeaks;

        protected Climber(string name, int stamina)
        {
            Name = name;
            Stamina = stamina;
            conqueredPeaks = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Climber's name cannot be null or whitespace.");
                }

                name = value;
            }
        }

        public int Stamina
        {
            get => stamina;
            protected set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > 10)
                {
                    value = 10;
                }

                stamina = value;
            }
        }

        public IReadOnlyCollection<string> ConqueredPeaks
            => conqueredPeaks.AsReadOnly();

        public void Climb(IPeak peak)
        {
            if (conqueredPeaks.Contains(peak.Name) == false)
            {
                conqueredPeaks.Add(peak.Name);
            }

            if (peak.DifficultyLevel == "Extreme")
            {
                Stamina -= 6;
            }
            else if (peak.DifficultyLevel == "Hard")
            {
                Stamina -= 4;
            }
            else if (peak.DifficultyLevel == "Moderate")
            {
                Stamina -= 2;
            }
        }

        public abstract void Rest(int daysCount);

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine($"{GetType().Name} - Name: {Name}, Stamina: {Stamina}");
            builder.Append("Peaks conquered: ");

            if (ConqueredPeaks.Any())
            {
                builder.AppendLine(ConqueredPeaks.Count.ToString());
            }
            else
            {
                builder.AppendLine("no peaks conquered");
            }

            return builder.ToString().TrimEnd();
        }
    }
}
