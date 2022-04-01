using StockTrader.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace StockTrader.EntityFramework
{
    /// <summary>
    /// AlgoTrading database context
    /// </summary>
    public class StockTraderDbContext : DbContext
    {
        #region Sets

        public DbSet<Log> Logs { get; set; }

        #endregion Sets

        /// <summary>
        /// Base creation for options
        /// </summary>
        /// <param name="options">Connection</param>
        public StockTraderDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
