using System.Collections.Generic;

namespace StockTrader.Domain.Models
{
    /// <summary>
    /// Brokeage account information
    /// </summary>
    public class Account : DatabaseObject
    {
        /// <summary>
        /// The user authentication information
        /// </summary>
        public User AccountHolder { get; set; }

        /// <summary>
        /// Amount of money user has
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Collections of transactions made by user
        /// </summary>
        public ICollection<AssetTransaction> AssetTransactions { get; set; }
    }
}