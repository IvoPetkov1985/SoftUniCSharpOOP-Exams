namespace Handball.Models.Players
{
    public class ForwardWing : Player
    {
        private const double ForwardWingInitialRating = 5.5;

        public ForwardWing(string name)
            : base(name, ForwardWingInitialRating)
        {
        }

        public override void DecreaseRating()
        {
            Rating -= 0.75;

            if (Rating < 1)
            {
                Rating = 1;
            }
        }

        public override void IncreaseRating()
        {
            Rating += 1.25;

            if (Rating > 10)
            {
                Rating = 10;
            }
        }
    }
}
