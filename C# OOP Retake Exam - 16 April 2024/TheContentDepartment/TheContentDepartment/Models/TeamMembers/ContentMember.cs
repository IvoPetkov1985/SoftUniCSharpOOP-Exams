using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheContentDepartment.Models.TeamMembers
{
    public class ContentMember : TeamMember
    {
        private string path;
        private readonly IEnumerable<string> ContentMemberPathValues
            = new List<string>() { "CSharp", "JavaScript", "Python", "Java" };

        public ContentMember(string name, string path)
            : base(name, path)
        {
        }

        public override string Path
        {
            get => path;
            protected set
            {
                if (!ContentMemberPathValues.Contains(value))
                {
                    throw new ArgumentException($"{path} path is not valid.");
                }

                path = value;
            }
        }

        public override string ToString()
        {
            return $"{Name} - {Path} path. Currently working on {InProgress.Count} tasks.";
        }
    }
}
