using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public class ScubaDiver : Diver
    {
        private const int ScubaDiverOxLevel = 540;

        public ScubaDiver(string name)
            : base(name, ScubaDiverOxLevel)
        {
        }

        public override void Miss(int timeToCatch)
        {
            int val = (int)Math.Round(timeToCatch * 0.3, MidpointRounding.AwayFromZero);
            OxygenLevel -= val;
        }

        public override void RenewOxy()
        {
            OxygenLevel = ScubaDiverOxLevel;
        }
    }
}
