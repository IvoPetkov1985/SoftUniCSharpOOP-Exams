using HighwayToPeak.Core.Contracts;
using HighwayToPeak.Models;
using HighwayToPeak.Models.Contracts;
using HighwayToPeak.Repositories;
using HighwayToPeak.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IPeak> peaks;
        private readonly IRepository<IClimber> climbers;
        private readonly IBaseCamp baseCamp;
        private readonly IEnumerable<string> acceptedDifficulties =
            new List<string>() { "Extreme", "Hard", "Moderate" };

        public Controller()
        {
            peaks = new PeakRepository();
            climbers = new ClimberRepository();
            baseCamp = new BaseCamp();
        }

        public string AddPeak(string name, int elevation, string difficultyLevel)
        {
            if (peaks.Get(name) != null)
            {
                return $"{name} is already added as a valid mountain destination.";
            }

            if (acceptedDifficulties.Contains(difficultyLevel) == false)
            {
                return $"{difficultyLevel} peaks are not allowed for international climbers.";
            }

            IPeak peak = new Peak(name, elevation, difficultyLevel);
            peaks.Add(peak);
            return $"{name} is allowed for international climbing. See details in {peaks.GetType().Name}.";
        }

        public string AttackPeak(string climberName, string peakName)
        {
            if (climbers.Get(climberName) == null)
            {
                return $"Climber - {climberName}, has not arrived at the BaseCamp yet.";
            }

            if (peaks.Get(peakName) == null)
            {
                return $"{peakName} is not allowed for international climbing.";
            }

            if (baseCamp.Residents.Contains(climberName) == false)
            {
                return $"{climberName} not found for gearing and instructions. The attack of {peakName} will be postponed.";
            }

            IClimber climber = climbers.Get(climberName);
            IPeak peak = peaks.Get(peakName);

            if (climber.GetType().Name == nameof(NaturalClimber)
                && peak.DifficultyLevel == "Extreme")
            {
                return $"{climberName} does not cover the requirements for climbing {peakName}.";
            }

            baseCamp.LeaveCamp(climberName);
            climber.Climb(peak);

            if (climber.Stamina <= 0)
            {
                return $"{climberName} did not return to BaseCamp.";
            }
            else
            {
                baseCamp.ArriveAtCamp(climberName);
                return $"{climberName} successfully conquered {peakName} and returned to BaseCamp.";
            }
        }

        public string BaseCampReport()
        {
            StringBuilder builder = new();

            if (baseCamp.Residents.Any())
            {
                builder.AppendLine("BaseCamp residents:");

                foreach (string name in baseCamp.Residents)
                {
                    IClimber climber = climbers.Get(name);
                    builder.AppendLine($"Name: {climber.Name}, Stamina: {climber.Stamina}, Count of Conquered Peaks: {climber.ConqueredPeaks.Count}");
                }
            }
            else
            {
                builder.AppendLine("BaseCamp is currently empty.");
            }

            return builder.ToString().TrimEnd();
        }

        public string CampRecovery(string climberName, int daysToRecover)
        {
            if (baseCamp.Residents.Contains(climberName) == false)
            {
                return $"{climberName} not found at the BaseCamp.";
            }

            IClimber climber = climbers.Get(climberName);

            if (climber.Stamina == 10)
            {
                return $"{climberName} has no need of recovery.";
            }

            climber.Rest(daysToRecover);
            return $"{climberName} has been recovering for {daysToRecover} days and is ready to attack the mountain.";
        }

        public string NewClimberAtCamp(string name, bool isOxygenUsed)
        {
            if (climbers.Get(name) != null)
            {
                return $"{name} is a participant in {climbers.GetType().Name} and cannot be duplicated.";
            }

            IClimber climber = null;

            if (isOxygenUsed)
            {
                climber = new OxygenClimber(name);
            }
            else
            {
                climber = new NaturalClimber(name);
            }

            climbers.Add(climber);
            baseCamp.ArriveAtCamp(climber.Name);
            return $"{name} has arrived at the BaseCamp and will wait for the best conditions.";
        }

        public string OverallStatistics()
        {
            IEnumerable<IClimber> orderedClimbers = climbers.All
                .OrderByDescending(c => c.ConqueredPeaks.Count)
                .ThenBy(c => c.Name);

            StringBuilder builder = new();
            builder.AppendLine("***Highway-To-Peak***");

            foreach (IClimber climber in orderedClimbers)
            {
                builder.AppendLine(climber.ToString());

                if (climber.ConqueredPeaks.Any())
                {
                    ICollection<IPeak> orderedPeaks = new List<IPeak>();

                    foreach (string peakName in climber.ConqueredPeaks)
                    {
                        IPeak peak = peaks.Get(peakName);
                        orderedPeaks.Add(peak);
                    }

                    foreach (IPeak peak in orderedPeaks
                        .OrderByDescending(p => p.Elevation))
                    {
                        builder.AppendLine(peak.ToString());
                    }
                }
            }

            return builder.ToString().TrimEnd();
        }
    }
}
