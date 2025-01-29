using HighwayToPeak.Models.Contracts;
using HighwayToPeak.Repositories.Contracts;

namespace HighwayToPeak.Repositories
{
    public class ClimberRepository : IRepository<IClimber>
    {
        private readonly List<IClimber> climbers;

        public ClimberRepository()
        {
            climbers = new List<IClimber>();
        }

        public IReadOnlyCollection<IClimber> All
            => climbers.AsReadOnly();

        public void Add(IClimber model)
        {
            climbers.Add(model);
        }

        public IClimber Get(string name)
        {
            return climbers.FirstOrDefault(c => c.Name == name);
        }
    }
}
