using System;

namespace StockTrader.Domain.Models
{
    /// <summary>
    /// Transaction information for any trade
    /// </summary>
    public class AssetTransaction : DatabaseObject
    {
        /// <summary>
        /// Account trade belongs to
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Selling or Buying
        /// </summary>
        public bool IsPurchase { get; set; }

        /// <summary>
        /// The stock information
        /// </summary>
        public Asset Asset { get; set; }

        /// <summary>
        /// Number of shares bought or sold
        /// </summary>
        public int Shares { get; set; }

        /// <summary>
        /// Time of date processed
        /// </summary>
        public DateTime DateProcessed { get; set; }
    }
}