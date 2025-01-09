using RobotService.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotService.Models
{
    public abstract class Robot : IRobot
    {
        private string model;
        private int batteryCapacity;
        private readonly List<int> interfaceStandards;

        protected Robot(string model, int batteryCapacity, int conversionCapacityIndex)
        {
            Model = model;
            BatteryCapacity = batteryCapacity;
            ConvertionCapacityIndex = conversionCapacityIndex;
            BatteryLevel = BatteryCapacity;
            interfaceStandards = new List<int>();
        }

        public string Model
        {
            get => model;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Model cannot be null or empty.");
                }

                model = value;
            }
        }

        public int BatteryCapacity
        {
            get => batteryCapacity;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Battery capacity cannot drop below zero.");
                }

                batteryCapacity = value;
            }
        }

        public int BatteryLevel { get; private set; }

        public int ConvertionCapacityIndex { get; private set; }

        public IReadOnlyCollection<int> InterfaceStandards { get => interfaceStandards.AsReadOnly(); }

        public void Eating(int minutes)
        {
            int energy = minutes * ConvertionCapacityIndex;
            BatteryLevel += energy;

            if (BatteryLevel > BatteryCapacity)
            {
                BatteryLevel = BatteryCapacity;
            }
        }

        public bool ExecuteService(int consumedEnergy)
        {
            if (BatteryLevel >= consumedEnergy)
            {
                BatteryLevel -= consumedEnergy;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InstallSupplement(ISupplement supplement)
        {
            interfaceStandards.Add(supplement.InterfaceStandard);
            BatteryCapacity -= supplement.BatteryUsage;
            BatteryLevel -= supplement.BatteryUsage;
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine($"{this.GetType().Name} {Model}:");
            builder.AppendLine($"--Maximum battery capacity: {BatteryCapacity}");
            builder.AppendLine($"--Current battery level: {BatteryLevel}");
            builder.AppendLine($"--Supplements installed: {(interfaceStandards.Any() ? string.Join(" ", interfaceStandards) : "none")}");

            return builder.ToString().TrimEnd();
        }
    }
}
