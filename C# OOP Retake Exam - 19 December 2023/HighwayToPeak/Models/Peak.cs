using HighwayToPeak.Models.Contracts;

namespace HighwayToPeak.Models
{
    public class Peak : IPeak
    {
        private string name;
        private int elevation;

        public Peak(string name, int elevation, string difficultyLevel)
        {
            Name = name;
            Elevation = elevation;
            DifficultyLevel = difficultyLevel;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Peak name cannot be null or whitespace.");
                }

                name = value;
            }
        }

        public int Elevation
        {
            get => elevation;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Peak elevation must be a positive value.");
                }

                elevation = value;
            }
        }

        public string DifficultyLevel { get; private set; }

        public override string ToString()
        {
            return $"Peak: {Name} -> Elevation: {Elevation}, Difficulty: {DifficultyLevel}";
        }
    }
}
