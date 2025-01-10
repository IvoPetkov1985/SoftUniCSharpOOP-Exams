namespace BankLoan.Models.Clients
{
    public class Adult : Client
    {
        private const int AdultInterest = 4;

        public Adult(string name, string id, double income) 
            : base(name, id, AdultInterest, income)
        {
        }

        public override void IncreaseInterest()
        {
            Interest += 2;
        }
    }
}
