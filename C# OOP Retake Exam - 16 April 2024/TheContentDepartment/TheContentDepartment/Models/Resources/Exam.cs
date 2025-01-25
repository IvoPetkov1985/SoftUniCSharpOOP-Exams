namespace TheContentDepartment.Models.Resources
{
    public class Exam : Resource
    {
        private const int ExamPriority = 1;

        public Exam(string name, string creator)
            : base(name, creator, ExamPriority)
        {
        }
    }
}
