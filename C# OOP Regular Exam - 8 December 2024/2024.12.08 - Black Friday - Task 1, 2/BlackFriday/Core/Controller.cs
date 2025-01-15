using BlackFriday.Core.Contracts;
using BlackFriday.Models.Apps;
using BlackFriday.Models.Contracts;
using BlackFriday.Models.Products;
using BlackFriday.Models.Users;
using BlackFriday.Utilities.Messages;
using System.Diagnostics;
using System.Text;

namespace BlackFriday.Core
{
    public class Controller : IController
    {
        private readonly IApplication application;

        public Controller()
        {
            application = new Application();
        }

        public string AddProduct(string productType, string productName, string userName, double basePrice)
        {
            if (productType != nameof(Item) && productType != nameof(Service))
            {
                return $"{productType} is not a valid type for the application.";
            }

            if (application.Products.Exists(productName))
            {
                return $"{productName} already exists in the application.";
            }

            if (application.Users.Exists(userName) == false ||
                application.Users.GetByName(userName).GetType().Name == nameof(Client))
            {
                return $"{userName} has no data access.";
            }

            IProduct product = null;

            if (productType == nameof(Item))
            {
                product = new Item(productName, basePrice);
            }
            else
            {
                product = new Service(productName, basePrice);
            }

            application.Products.AddNew(product);
            return $"{productType}: {productName} is added in the application. Price: {basePrice:F2}";
        }

        public string ApplicationReport()
        {
            StringBuilder builder = new();

            builder.AppendLine("Application administration:");

            IOrderedEnumerable<IUser> admins = application.Users.Models
                .Where(u => u.GetType().Name == nameof(Admin))
                .OrderBy(u => u.UserName);

            foreach (IUser user in admins)
            {
                builder.AppendLine(user.ToString());
            }

            builder.AppendLine("Clients:");

            IOrderedEnumerable<Client> clients = application.Users.Models
                .Where(u => u.GetType().Name == nameof(Client))
                .Select(x => (Client)x)
                .OrderBy(c => c.UserName);

            foreach (var client in clients)
            {
                builder.AppendLine(client.ToString());

                if (client.Purchases.Any(p => p.Value == true))
                {
                    Dictionary<string, bool> purchasedPromotions = client.Purchases.Where(p => p.Value == true)
                        .ToDictionary(x => x.Key, y => y.Value);
                    builder.AppendLine($"-Black Friday Purchases: {purchasedPromotions.Count}");

                    foreach (KeyValuePair<string, bool> product in purchasedPromotions)
                    {
                        builder.AppendLine("--" + product.Key);
                    }
                }
            }

            return builder.ToString().TrimEnd();
        }

        public string PurchaseProduct(string userName, string productName, bool blackFridayFlag)
        {
            if (application.Users.Exists(userName) == false ||
                application.Users.GetByName(userName).GetType().Name == nameof(Admin))
            {
                return $"{userName} has no authorization for this functionality.";
            }

            if (application.Products.Exists(productName) == false)
            {
                return $"{productName} does not exist in the application.";
            }

            if (application.Products.GetByName(productName).IsSold == true)
            {
                return $"{productName} is out of stock.";
            }

            IProduct product = application.Products.GetByName(productName);
            Client client = (Client)application.Users.GetByName(userName);
            client.PurchaseProduct(productName, blackFridayFlag);
            product.ToggleStatus();

            if (blackFridayFlag == true)
            {
                return $"{userName} purchased {productName}. Price: {product.BlackFridayPrice:F2}";
            }
            else
            {
                return $"{userName} purchased {productName}. Price: {product.BasePrice:F2}";
            }
        }

        public string RefreshSalesList(string userName)
        {
            if (application.Users.Exists(userName) == false ||
                application.Users.GetByName(userName).GetType().Name == nameof(Client))
            {
                return $"{userName} has no data access.";
            }

            IEnumerable<IProduct> soldProducts = application
                .Products.Models.Where(p => p.IsSold == true);

            int count = soldProducts.Count();

            foreach (IProduct product in soldProducts)
            {
                product.ToggleStatus();
            }

            return $"{count} products are listed again.";
        }

        public string RegisterUser(string userName, string email, bool hasDataAccess)
        {
            if (application.Users.Exists(userName))
            {
                return $"{userName} is already registered.";
            }

            if (application.Users.Models.Any(u => u.Email == email))
            {
                return $"{email} is already used by another user.";
            }

            if (hasDataAccess == true)
            {
                if (application.Users.Models.Where(u => u.HasDataAccess == true).Count() == 2)
                {
                    return "The number of application administrators is limited.";
                }
                else
                {
                    IUser user = new Admin(userName, email);
                    application.Users.AddNew(user);
                    return $"Admin {userName} is successfully registered with data access.";
                }
            }
            else
            {
                IUser user = new Client(userName, email);
                application.Users.AddNew(user);
                return $"Client {userName} is successfully registered.";
            }
        }

        public string UpdateProductPrice(string productName, string userName, double newPriceValue)
        {
            if (application.Products.Exists(productName) == false)
            {
                return $"{productName} does not exist in the application.";
            }

            if (application.Users.Exists(userName) == false ||
                application.Users.GetByName(userName).GetType().Name == nameof(Client))
            {
                return $"{userName} has no data access.";
            }

            IProduct product = application.Products.GetByName(productName);
            double oldPriceValue = product.BasePrice;
            product.UpdatePrice(newPriceValue);
            return $"{productName} -> Price is updated: {oldPriceValue:F2} -> {newPriceValue:F2}";
        }
    }
}
