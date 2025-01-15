using BlackFriday.Models.Contracts;

namespace BlackFriday.Models.Products
{
    public abstract class Product : IProduct
    {
        private string productName;
        private double basePrice;

        protected Product(string productName, double basePrice)
        {
            ProductName = productName;
            BasePrice = basePrice;
            IsSold = false;
        }

        public string ProductName
        {
            get => productName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Product name is required.");
                }

                productName = value;
            }
        }

        public double BasePrice
        {
            get => basePrice;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Price cannot be zero or negative.");
                }

                basePrice = value;
            }
        }

        public virtual double BlackFridayPrice { get; }

        public bool IsSold { get; private set; }

        public void ToggleStatus()
        {
            IsSold = !IsSold;
        }

        public void UpdatePrice(double newPriceValue)
        {
            BasePrice = newPriceValue;
        }

        public override string ToString()
        {
            return $"Product: {ProductName}, Price: {BasePrice:F2}, You Save: {(BasePrice - BlackFridayPrice):F2}";
        }
    }
}
