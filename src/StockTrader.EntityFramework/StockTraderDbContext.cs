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

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AssetTransaction> AssetTransactions { get; set; }

        #endregion Sets

        /// <summary>
        /// Base creation for options
        /// </summary>
        /// <param name="options">Connection</param>
        public StockTraderDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Build relations on models
        /// </summary>
        /// <param name="modelBuilder">base builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetTransaction>().OwnsOne(a => a.Asset);
            base.OnModelCreating(modelBuilder);
        }
    }
}
