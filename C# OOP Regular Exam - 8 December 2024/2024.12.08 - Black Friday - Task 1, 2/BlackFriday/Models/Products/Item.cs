namespace BlackFriday.Models.Products
{
    public class Item : Product
    {
        public Item(string productName, double basePrice)
            : base(productName, basePrice)
        {
        }

        public override double BlackFridayPrice
            => BasePrice * 0.7;
    }
}
