using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheContentDepartment.Models.Resources
{
    public class Presentation : Resource
    {
        private const int PresentationPriority = 3;

        public Presentation(string name, string creator)
            : base(name, creator, PresentationPriority)
        {
        }
    }
}
