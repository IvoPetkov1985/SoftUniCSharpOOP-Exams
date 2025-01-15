namespace BlackFriday.Models.Users
{
    public class Client : User
    {
        private readonly Dictionary<string, bool> purchases;

        public Client(string userName, string email)
            : base(userName, email, false)
        {
            purchases = new Dictionary<string, bool>();
        }

        public IReadOnlyDictionary<string, bool> Purchases
            => purchases;

        public void PurchaseProduct(string productName, bool blackFridayFlag)
        {
            purchases.Add(productName, blackFridayFlag);
        }
    }
}
