namespace BankLoan.Models.Clients
{
    public class Student : Client
    {
        private const int StudentInterest = 2;

        public Student(string name, string id, double income)
            : base(name, id, StudentInterest, income)
        {
        }

        public override void IncreaseInterest()
        {
            Interest += 1;
        }
    }
}
