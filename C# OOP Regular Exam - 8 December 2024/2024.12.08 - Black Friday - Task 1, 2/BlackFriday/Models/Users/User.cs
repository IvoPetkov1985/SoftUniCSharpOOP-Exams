using BlackFriday.Models.Contracts;

namespace BlackFriday.Models.Users
{
    public abstract class User : IUser
    {
        private string userName;
        private string email;

        protected User(string userName, string email, bool hasDataAccess)
        {
            UserName = userName;
            HasDataAccess = hasDataAccess;
            Email = email;
        }

        public string UserName
        {
            get => userName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username is required.");
                }

                userName = value;
            }
        }

        public bool HasDataAccess { get; }

        public string Email
        {
            get => email;
            private set
            {
                if (HasDataAccess == true)
                {
                    value = "hidden";
                }

                if (string.IsNullOrWhiteSpace(value) && HasDataAccess == false)
                {
                    throw new ArgumentException("Email is required.");
                }

                email = value;
            }
        }

        public override string ToString()
        {
            return $"{UserName} - Status: {this.GetType().Name}, Contact Info: {Email}";
        }
    }
}
