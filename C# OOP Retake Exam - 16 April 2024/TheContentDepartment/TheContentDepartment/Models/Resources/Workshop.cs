using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheContentDepartment.Models.Resources
{
    public class Workshop : Resource
    {
        private const int WorkshopPriority = 2;

        public Workshop(string name, string creator)
            : base(name, creator, WorkshopPriority)
        {
        }
    }
}
