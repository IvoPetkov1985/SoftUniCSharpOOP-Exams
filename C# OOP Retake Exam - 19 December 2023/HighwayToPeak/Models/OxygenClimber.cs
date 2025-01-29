namespace HighwayToPeak.Models
{
    public class OxygenClimber : Climber
    {
        private const int OxygenClimberInitialStamina = 10;

        public OxygenClimber(string name)
            : base(name, OxygenClimberInitialStamina)
        {
        }

        public override void Rest(int daysCount)
        {
            Stamina += daysCount;
        }
    }
}
