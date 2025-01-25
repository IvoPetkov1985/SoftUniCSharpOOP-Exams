using System.Text;
using TheContentDepartment.Core.Contracts;
using TheContentDepartment.Models.Contracts;
using TheContentDepartment.Models.Resources;
using TheContentDepartment.Models.TeamMembers;
using TheContentDepartment.Repositories;
using TheContentDepartment.Repositories.Contracts;

namespace TheContentDepartment.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IResource> resources;
        private readonly IRepository<ITeamMember> members;

        public Controller()
        {
            resources = new ResourceRepository();
            members = new MemberRepository();
        }

        public string ApproveResource(string resourceName, bool isApprovedByTeamLead)
        {
            IResource resource = resources.TakeOne(resourceName);

            if (resource.IsTested == false)
            {
                return $"{resourceName} cannot be approved without being tested.";
            }

            ITeamMember teamLead = members.Models.FirstOrDefault(tl => tl.GetType().Name == nameof(TeamLead));

            if (isApprovedByTeamLead)
            {
                resource.Approve();
                teamLead.FinishTask(resource.Name);
                return $"{teamLead.Name} approved {resourceName}.";
            }
            else
            {
                resource.Test();
                return $"{teamLead.Name} returned {resourceName} for a re-test.";
            }
        }

        public string CreateResource(string resourceType, string resourceName, string path)
        {
            if (resourceType != nameof(Exam) &&
                resourceType != nameof(Workshop) &&
                resourceType != nameof(Presentation))
            {
                return $"{resourceType} type is not handled by Content Department.";
            }

            ITeamMember member = members.Models.FirstOrDefault(tm => tm.GetType().Name == nameof(ContentMember) && tm.Path == path);

            if (member == null)
            {
                return $"No content member is able to create the {resourceName} resource.";
            }

            if (member.InProgress.Contains(resourceName))
            {
                return $"The {resourceName} resource is being created.";
            }

            IResource resource = null;

            if (resourceType == nameof(Exam))
            {
                resource = new Exam(resourceName, member.Name);
            }
            else if (resourceType == nameof(Workshop))
            {
                resource = new Workshop(resourceName, member.Name);
            }
            else
            {
                resource = new Presentation(resourceName, member.Name);
            }

            member.WorkOnTask(resourceName);
            resources.Add(resource);
            return $"{member.Name} created {resourceType} - {resourceName}.";
        }

        public string DepartmentReport()
        {
            StringBuilder builder = new();
            builder.AppendLine("Finished Tasks:");

            foreach (IResource resource in resources.Models
                .Where(r => r.IsApproved))
            {
                builder.AppendLine($"--{resource.ToString()}");
            }

            builder.AppendLine("Team Report:");
            ITeamMember teamLead = members.Models.FirstOrDefault(tl => tl.GetType().Name == nameof(TeamLead));
            builder.AppendLine($"--{teamLead.ToString()}");

            foreach (ITeamMember member in members.Models
                .Where(m => m.GetType().Name == nameof(ContentMember)))
            {
                builder.AppendLine(member.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string JoinTeam(string memberType, string memberName, string path)
        {
            if (memberType != nameof(TeamLead) && memberType != nameof(ContentMember))
            {
                return $"{memberType} is not a valid member type.";
            }

            if (members.Models.Any(tm => tm.Path == path))
            {
                return "Position is occupied.";
            }

            ITeamMember member = members.TakeOne(memberName);

            if (member != null)
            {
                return $"{memberName} has already joined the team.";
            }

            if (memberType == nameof(TeamLead))
            {
                member = new TeamLead(memberName, path);
            }
            else
            {
                member = new ContentMember(memberName, path);
            }

            members.Add(member);
            return $"{memberName} has joined the team. Welcome!";
        }

        public string LogTesting(string memberName)
        {
            ITeamMember member = members.TakeOne(memberName);

            if (member == null)
            {
                return "Provide the correct member name.";
            }

            IResource resource = resources.Models
                .OrderBy(r => r.Priority)
                .Where(r => r.IsTested == false && r.Creator == memberName)
                .FirstOrDefault();

            if (resource == null)
            {
                return $"{memberName} has no resources for testing.";
            }

            ITeamMember teamLead = members.Models.FirstOrDefault(tl => tl.GetType().Name == nameof(TeamLead));

            member.FinishTask(resource.Name);
            teamLead.WorkOnTask(resource.Name);
            resource.Test();
            return $"{resource.Name} is tested and ready for approval.";
        }
    }
}
