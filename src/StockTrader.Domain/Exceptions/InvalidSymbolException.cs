using System;

namespace StockTrader.Domain.Exceptions
{
    /// <summary>
    /// Occurs when a symbol is not in the NASDAQ database
    /// </summary>
    public class InvalidSymbolException : Exception
    {
        /// <summary>
        /// The stock symbol
        /// </summary>
        public string Symbol { get; set; }

        public InvalidSymbolException(string symbol)
        {
            Symbol = symbol;
        }

        public InvalidSymbolException(string symbol, string message) : base(message)
        {
            Symbol = symbol;
        }

        public InvalidSymbolException(string symbol, string message, Exception innerException) : base(message, innerException)
        {
            Symbol = symbol;
        }
    }
}