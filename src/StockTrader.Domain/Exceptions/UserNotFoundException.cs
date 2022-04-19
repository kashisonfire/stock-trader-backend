using System;

namespace StockTrader.Domain.Exceptions
{
    /// <summary>
    /// Occurs when a username is not found in the database
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Username of the account
        /// </summary>
        public string UsernameOrEmail { get; set; }

        public UserNotFoundException(string usernameOrEmail)
        {
            UsernameOrEmail = usernameOrEmail;
        }

        public UserNotFoundException(string message, string usernameOrEmail) : base(message)
        {
            UsernameOrEmail = usernameOrEmail;
        }

        public UserNotFoundException(string message, Exception innerException, string usernameOrEmail) : base(message, innerException)
        {
            UsernameOrEmail = usernameOrEmail;
        }
    }
}