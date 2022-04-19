using System;

namespace StockTrader.Domain.Exceptions
{
    /// <summary>
    /// Occurs when a user has invalid username or password
    /// </summary>
    public class InvalidPasswordException : Exception
    {
        /// <summary>
        /// User's username
        /// </summary>
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }

        public InvalidPasswordException(string usernameOrEmail, string password)
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }

        public InvalidPasswordException(string message, string usernameOrEmail, string password) : base(message)
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }

        public InvalidPasswordException(string message, Exception innerException, string usernameOrEmail, string password) : base(message, innerException)
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }
    }
}