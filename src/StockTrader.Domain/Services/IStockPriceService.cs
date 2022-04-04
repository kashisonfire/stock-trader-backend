using StockTrader.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace StockTrader.Domain.Services
{
    public interface IStockPriceService
    {
        /// <summary>
        /// Get the share price for a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to get the price of.</param>
        /// <returns>The price of symbol.</returns>
        /// <exception cref="InvalidSymbolException">Thrown if symbol does not exist.</exception>
        /// <exception cref="Exception">Thrown if getting the symbol fails.</exception>
        Task<double> GetPrice(string symbol);
    }
}