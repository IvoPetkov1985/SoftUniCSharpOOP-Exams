using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public class FreeDiver : Diver
    {
        private const int FreeDiverOxLevel = 120;

        public FreeDiver(string name)
            : base(name, FreeDiverOxLevel)
        {
        }

        public override void Miss(int timeToCatch)
        {
            int val = (int)Math.Round(timeToCatch * 0.6, MidpointRounding.AwayFromZero);
            OxygenLevel -= val;
        }

        public override void RenewOxy()
        {
            OxygenLevel = FreeDiverOxLevel;
        }
    }
}
