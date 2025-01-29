namespace HighwayToPeak.Models
{
    public class NaturalClimber : Climber
    {
        private const int NaturalClimberInitialStamina = 6;

        public NaturalClimber(string name)
            : base(name, NaturalClimberInitialStamina)
        {
        }

        public override void Rest(int daysCount)
        {
            Stamina += daysCount * 2;
        }
    }
}
