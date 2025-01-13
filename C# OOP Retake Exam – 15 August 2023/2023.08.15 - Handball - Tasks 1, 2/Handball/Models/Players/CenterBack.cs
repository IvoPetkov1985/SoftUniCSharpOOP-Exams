namespace Handball.Models.Players
{
    public class CenterBack : Player
    {
        private const double CenterBackInitialRating = 4;

        public CenterBack(string name)
            : base(name, CenterBackInitialRating)
        {
        }

        public override void DecreaseRating()
        {
            Rating -= 1;

            if (Rating < 1)
            {
                Rating = 1;
            }
        }

        public override void IncreaseRating()
        {
            Rating += 1;

            if (Rating > 10)
            {
                Rating = 10;
            }
        }
    }
}
