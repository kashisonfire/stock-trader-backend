using System;

namespace StockTrader.Domain.Exceptions
{
    /// <summary>
    /// Occurs when a user purchases a stock with an insufficient balance
    /// </summary>
    public class InsufficientFundsException : Exception
    {
        /// <summary>
        /// Current account balance
        /// </summary>
        public double AccountBalance { get; set; }

        /// <summary>
        /// Balance required to make transaction
        /// </summary>
        public double RequiredBalance { get; set; }

        public InsufficientFundsException(double accountBalance, double requiredBalance)
        {
            AccountBalance = accountBalance;
            RequiredBalance = requiredBalance;
        }

        public InsufficientFundsException(double accountBalance, double requiredBalance, string message) : base(message)
        {
            AccountBalance = accountBalance;
            RequiredBalance = requiredBalance;
        }

        public InsufficientFundsException(double accountBalance, double requiredBalance, string message, Exception innerException) : base(message, innerException)
        {
            AccountBalance = accountBalance;
            RequiredBalance = requiredBalance;
        }
    }
}