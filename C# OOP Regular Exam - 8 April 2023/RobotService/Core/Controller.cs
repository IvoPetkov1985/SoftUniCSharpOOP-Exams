using RobotService.Core.Contracts;
using RobotService.Models.Contracts;
using RobotService.Models.Robots;
using RobotService.Models.Supplements;
using RobotService.Repositories;
using RobotService.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotService.Core
{
    public class Controller : IController
    {
        private readonly IRepository<ISupplement> supplements;
        private readonly IRepository<IRobot> robots;

        public Controller()
        {
            supplements = new SupplementRepository();
            robots = new RobotRepository();
        }

        public string CreateRobot(string model, string typeName)
        {
            if (typeName != nameof(DomesticAssistant) &&
                typeName != nameof(IndustrialAssistant))
            {
                return $"Robot type {typeName} cannot be created.";
            }

            IRobot robot = null;

            if (typeName == nameof(DomesticAssistant))
            {
                robot = new DomesticAssistant(model);
            }
            else
            {
                robot = new IndustrialAssistant(model);
            }

            robots.AddNew(robot);
            return $"{typeName} {model} is created and added to the RobotRepository.";
        }

        public string CreateSupplement(string typeName)
        {
            if (typeName != nameof(SpecializedArm) &&
                typeName != nameof(LaserRadar))
            {
                return $"{typeName} is not compatible with our robots.";
            }

            ISupplement supplement = null;

            if (typeName == nameof(SpecializedArm))
            {
                supplement = new SpecializedArm();
            }
            else
            {
                supplement = new LaserRadar();
            }

            supplements.AddNew(supplement);
            return $"{typeName} is created and added to the SupplementRepository.";
        }

        public string PerformService(string serviceName, int intefaceStandard, int totalPowerNeeded)
        {
            IEnumerable<IRobot> selectedRobots = robots.Models()
                .Where(r => r.InterfaceStandards.Contains(intefaceStandard))
                .OrderByDescending(r => r.BatteryLevel);

            if (!selectedRobots.Any())
            {
                return $"Unable to perform service, {intefaceStandard} not supported!";
            }

            int sumEnergy = selectedRobots.Sum(r => r.BatteryLevel);

            if (sumEnergy < totalPowerNeeded)
            {
                return $"{serviceName} cannot be executed! {totalPowerNeeded - sumEnergy} more power needed.";
            }

            int counter = 0;

            while (totalPowerNeeded > 0)
            {
                foreach (IRobot robot in selectedRobots)
                {
                    if (robot.BatteryLevel >= totalPowerNeeded)
                    {
                        robot.ExecuteService(totalPowerNeeded);
                        totalPowerNeeded = 0;
                        counter++;
                        break;
                    }
                    else
                    {
                        totalPowerNeeded -= robot.BatteryLevel;
                        robot.ExecuteService(robot.BatteryLevel);
                        counter++;
                    }
                }
            }

            return $"{serviceName} is performed successfully with {counter} robots.";
        }

        public string Report()
        {
            StringBuilder builder = new();

            foreach (IRobot robot in robots.Models()
                .OrderByDescending(r => r.BatteryLevel)
                .ThenBy(r => r.BatteryCapacity))
            {
                builder.AppendLine(robot.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string RobotRecovery(string model, int minutes)
        {
            IEnumerable<IRobot> robotsToFeed = robots.Models()
                .Where(r => r.Model == model && r.BatteryLevel < r.BatteryCapacity / 2);

            int count = robotsToFeed.Count();

            foreach (IRobot robot in robotsToFeed)
            {
                robot.Eating(minutes);
            }

            return $"Robots fed: {count}";
        }

        public string UpgradeRobot(string model, string supplementTypeName)
        {
            ISupplement supplement = supplements.Models().FirstOrDefault(s => s.GetType().Name == supplementTypeName);
            int interfaceValue = supplement.InterfaceStandard;

            IEnumerable<IRobot> selectedRobots = robots.Models()
                .Where(r => r.Model == model && !r.InterfaceStandards.Contains(interfaceValue));

            if (!selectedRobots.Any())
            {
                return $"All {model} are already upgraded!";
            }

            IRobot robotToUpgrade = selectedRobots.First();
            robotToUpgrade.InstallSupplement(supplement);
            supplements.RemoveByName(supplement.GetType().Name);
            return $"{model} is upgraded with {supplementTypeName}.";
        }
    }
}
