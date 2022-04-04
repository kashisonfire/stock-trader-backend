using System;

namespace StockTrader.Domain.Exceptions
{
    /// <summary>
    /// Occurs when a user trades a stock with an insufficient number of shares
    /// </summary>
    public class InsufficientSharesException : Exception
    {
        /// <summary>
        /// Stock symbol
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Number of shares held by account
        /// </summary>
        public int AccountShares { get; }

        /// <summary>
        /// Number of required shares to make transaction
        /// </summary>
        public int RequiredShares { get; }

        public InsufficientSharesException(string symbol, int accountShares, int requiredShares)
        {
            Symbol = symbol;
            AccountShares = accountShares;
            RequiredShares = requiredShares;
        }

        public InsufficientSharesException(string symbol, int accountShares, int requiredShares, string message) : base(message)
        {
            Symbol = symbol;
            AccountShares = accountShares;
            RequiredShares = requiredShares;
        }

        public InsufficientSharesException(string symbol, int accountShares, int requiredShares, string message, Exception innerException) : base(message, innerException)
        {
            Symbol = symbol;
            AccountShares = accountShares;
            RequiredShares = requiredShares;
        }
    }
}