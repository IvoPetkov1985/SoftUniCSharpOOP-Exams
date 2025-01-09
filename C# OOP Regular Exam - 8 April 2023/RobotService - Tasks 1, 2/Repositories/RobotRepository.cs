using RobotService.Models.Contracts;
using RobotService.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotService.Repositories
{
    public class RobotRepository : IRepository<IRobot>
    {
        private readonly List<IRobot> robots;

        public RobotRepository()
        {
            robots = new List<IRobot>();
        }

        public void AddNew(IRobot robot)
        {
            robots.Add(robot);
        }

        public IRobot FindByStandard(int interfaceStandard)
        {
            IRobot robot = robots
                .FirstOrDefault(r => r.InterfaceStandards.Contains(interfaceStandard));
            return robot;
        }

        public IReadOnlyCollection<IRobot> Models()
        {
            return robots.AsReadOnly();
        }

        public bool RemoveByName(string robotModel)
        {
            IRobot robotToRemove = robots
                .FirstOrDefault(r => r.Model == robotModel);

            return robots.Remove(robotToRemove);
        }
    }
}
