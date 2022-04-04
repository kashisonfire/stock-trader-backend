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
        public string Username { get; set; }

        public UserNotFoundException(string username)
        {
            Username = username;
        }

        public UserNotFoundException(string message, string username) : base(message)
        {
            Username = username;
        }

        public UserNotFoundException(string message, Exception innerException, string username) : base(message, innerException)
        {
            Username = username;
        }
    }
}