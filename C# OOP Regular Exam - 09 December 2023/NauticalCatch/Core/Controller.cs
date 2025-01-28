using NauticalCatchChallenge.Core.Contracts;
using NauticalCatchChallenge.Models;
using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Repositories;
using NauticalCatchChallenge.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IDiver> divers;
        private readonly IRepository<IFish> fish;

        public Controller()
        {
            divers = new DiverRepository();
            fish = new FishRepository();
        }

        public string ChaseFish(string diverName, string fishName, bool isLucky)
        {
            IDiver diver = divers.GetModel(diverName);

            if (diver == null)
            {
                return $"{divers.GetType().Name} has no {diverName} registered for the competition.";
            }

            IFish searchedFish = fish.GetModel(fishName);

            if (searchedFish == null)
            {
                return $"{fishName} is not allowed to be caught in this competition.";
            }

            if (diver.HasHealthIssues == true)
            {
                return $"{diverName} will not be allowed to dive, due to health issues.";
            }

            bool isSuccessful = false;

            if (diver.OxygenLevel < searchedFish.TimeToCatch)
            {
                diver.Miss(searchedFish.TimeToCatch);
            }
            else if (diver.OxygenLevel == searchedFish.TimeToCatch)
            {
                if (isLucky == true)
                {
                    diver.Hit(searchedFish);
                    isSuccessful = true;
                }
                else
                {
                    diver.Miss(searchedFish.TimeToCatch);
                }
            }
            else
            {
                diver.Hit(searchedFish);
                isSuccessful = true;
            }

            if (diver.OxygenLevel <= 0)
            {
                diver.UpdateHealthStatus();
            }

            if (isSuccessful == true)
            {
                return $"{diverName} hits a {searchedFish.Points}pt. {fishName}.";
            }
            else
            {
                return $"{diverName} missed a good {fishName}.";
            }
        }

        public string CompetitionStatistics()
        {
            StringBuilder builder = new();
            builder.AppendLine("**Nautical-Catch-Challenge**");

            IEnumerable<IDiver> selectedDivers = divers.Models
                .Where(d => d.HasHealthIssues == false)
                .OrderByDescending(d => d.CompetitionPoints)
                .ThenByDescending(d => d.Catch.Count)
                .ThenBy(d => d.Name);

            foreach (IDiver diver in selectedDivers)
            {
                builder.AppendLine(diver.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string DiveIntoCompetition(string diverType, string diverName)
        {
            if (diverType != nameof(FreeDiver) &&
                diverType != nameof(ScubaDiver))
            {
                return $"{diverType} is not allowed in our competition.";
            }

            IDiver diver = divers.GetModel(diverName);

            if (diver != null)
            {
                return $"{diverName} is already a participant -> {divers.GetType().Name}.";
            }

            if (diverType == nameof(FreeDiver))
            {
                diver = new FreeDiver(diverName);
            }
            else
            {
                diver = new ScubaDiver(diverName);
            }

            divers.AddModel(diver);
            return $"{diverName} is successfully registered for the competition -> {divers.GetType().Name}.";
        }

        public string DiverCatchReport(string diverName)
        {
            IDiver diver = divers.GetModel(diverName);

            StringBuilder builder = new();
            builder.AppendLine(diver.ToString());
            builder.AppendLine("Catch Report:");

            foreach (string coughtFish in diver.Catch)
            {
                IFish searchedFish = fish.GetModel(coughtFish);
                builder.AppendLine(searchedFish.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string HealthRecovery()
        {
            IEnumerable<IDiver> diversWithHealthIssues = divers.Models
                .Where(d => d.HasHealthIssues == true);

            int count = diversWithHealthIssues.Count();

            foreach (IDiver diver in diversWithHealthIssues)
            {
                diver.UpdateHealthStatus();
                diver.RenewOxy();
            }

            return $"Divers recovered: {count}";
        }

        public string SwimIntoCompetition(string fishType, string fishName, double points)
        {
            if (fishType != nameof(ReefFish) &&
                fishType != nameof(PredatoryFish) &&
                fishType != nameof(DeepSeaFish))
            {
                return $"{fishType} is forbidden for chasing in our competition.";
            }

            IFish fishToAdd = fish.GetModel(fishName);

            if (fishToAdd != null)
            {
                return $"{fishName} is already allowed -> {fish.GetType().Name}.";
            }

            if (fishType == nameof(ReefFish))
            {
                fishToAdd = new ReefFish(fishName, points);
            }
            else if (fishType == nameof(PredatoryFish))
            {
                fishToAdd = new PredatoryFish(fishName, points);
            }
            else
            {
                fishToAdd = new DeepSeaFish(fishName, points);
            }

            fish.AddModel(fishToAdd);
            return $"{fishName} is allowed for chasing.";
        }
    }
}
