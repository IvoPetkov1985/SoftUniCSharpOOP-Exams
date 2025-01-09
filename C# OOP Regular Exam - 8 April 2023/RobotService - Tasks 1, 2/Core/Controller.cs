using RobotService.Core.Contracts;
using RobotService.Models;
using RobotService.Models.Contracts;
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
            if (typeName != nameof(DomesticAssistant) && typeName != nameof(IndustrialAssistant))
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
            if (typeName != nameof(SpecializedArm) && typeName != nameof(LaserRadar))
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
            List<IRobot> selectedRobots = robots.Models()
                .Where(r => r.InterfaceStandards.Contains(intefaceStandard))
                .OrderByDescending(r => r.BatteryLevel)
                .ToList();

            if (selectedRobots.Any() == false)
            {
                return $"Unable to perform service, {intefaceStandard} not supported!";
            }

            int batteryLevelSum = selectedRobots.Sum(r => r.BatteryLevel);

            if (batteryLevelSum < totalPowerNeeded)
            {
                return $"{serviceName} cannot be executed! {totalPowerNeeded - batteryLevelSum} more power needed.";
            }
            else
            {
                int robotsCount = 0;

                foreach (IRobot robot in selectedRobots)
                {
                    if (robot.BatteryLevel >= totalPowerNeeded)
                    {
                        robot.ExecuteService(totalPowerNeeded);
                        robotsCount++;
                        break;
                    }
                    else
                    {
                        totalPowerNeeded -= robot.BatteryLevel;
                        robot.ExecuteService(robot.BatteryLevel);
                        robotsCount++;
                    }
                }

                return $"{serviceName} is performed successfully with {robotsCount} robots.";
            }
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
            List<IRobot> selectedRobots = robots.Models()
                .Where(r => r.Model == model && r.BatteryLevel < r.BatteryCapacity / 2)
                .ToList();

            foreach (IRobot robot in selectedRobots)
            {
                robot.Eating(minutes);
            }

            return $"Robots fed: {selectedRobots.Count}";
        }

        public string UpgradeRobot(string model, string supplementTypeName)
        {
            ISupplement supplement = supplements.Models()
                .FirstOrDefault(s => s.GetType().Name == supplementTypeName);

            int interfaceValue = supplement.InterfaceStandard;

            List<IRobot> selectedRobots = robots.Models()
                .Where(r => r.InterfaceStandards.Contains(interfaceValue) == false && r.Model == model)
                .ToList();

            if (selectedRobots.Any() == false)
            {
                return $"All {model} are already upgraded!";
            }
            else
            {
                IRobot robot = selectedRobots.First();
                robot.InstallSupplement(supplement);
                supplements.RemoveByName(supplementTypeName);
                return $"{model} is upgraded with {supplementTypeName}.";
            }
        }
    }
}
