using TheContentDepartment.Models.Contracts;
using TheContentDepartment.Repositories.Contracts;

namespace TheContentDepartment.Repositories
{
    public class ResourceRepository : IRepository<IResource>
    {
        private readonly List<IResource> resources;

        public ResourceRepository()
        {
            resources = new List<IResource>();
        }

        public IReadOnlyCollection<IResource> Models
            => resources.AsReadOnly();

        public void Add(IResource model)
        {
            resources.Add(model);
        }

        public IResource TakeOne(string modelName)
        {
            IResource resource = resources.FirstOrDefault(r => r.Name == modelName);
            return resource;
        }
    }
}
