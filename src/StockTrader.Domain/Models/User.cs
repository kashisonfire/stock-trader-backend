using System;

namespace StockTrader.Domain.Models
{
    /// <summary>
    /// User for authentication
    /// </summary>
    public class User : DatabaseObject
    {
        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Hased password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Access level to software 
        /// </summary>
        public AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// Date joined
        /// </summary>
        public DateTime DateJoined { get; set; }
    }
}