namespace StockTrader.Domain.Models
{
    /// <summary>
    /// Stock asset NASDAQ
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Trading symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Price per share
        /// </summary>
        public double PricePerShare { get; set; }
    }
}