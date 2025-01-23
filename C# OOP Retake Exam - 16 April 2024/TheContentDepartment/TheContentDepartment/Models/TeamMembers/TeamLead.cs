using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheContentDepartment.Models.TeamMembers
{
    public class TeamLead : TeamMember
    {
        private const string TeamLeadPathValue = "Master";
        private string path;

        public TeamLead(string name, string path)
            : base(name, path)
        {
        }

        public override string Path
        {
            get => path;
            protected set
            {
                if (value != TeamLeadPathValue)
                {
                    throw new ArgumentException($"{path} path is not valid.");
                }

                path = value;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({this.GetType().Name}) - Currently working on {InProgress.Count} tasks.";
        }
    }
}
