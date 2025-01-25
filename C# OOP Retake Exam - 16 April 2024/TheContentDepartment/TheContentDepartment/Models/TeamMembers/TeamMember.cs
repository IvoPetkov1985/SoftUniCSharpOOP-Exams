using TheContentDepartment.Models.Contracts;

namespace TheContentDepartment.Models.TeamMembers
{
    public abstract class TeamMember : ITeamMember
    {
        private string name;
        private readonly List<string> inProgress;

        protected TeamMember(string name, string path)
        {
            Name = name;
            Path = path;
            inProgress = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or whitespace.");
                }

                name = value;
            }
        }

        public virtual string Path { get; protected set; }

        public IReadOnlyCollection<string> InProgress
            => inProgress.AsReadOnly();

        public void FinishTask(string resourceName)
        {
            inProgress.Remove(resourceName);
        }

        public void WorkOnTask(string resourceName)
        {
            inProgress.Add(resourceName);
        }
    }
}
