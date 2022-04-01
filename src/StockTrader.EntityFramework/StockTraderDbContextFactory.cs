using Microsoft.EntityFrameworkCore;
using System;

namespace StockTrader.EntityFramework
{
    public class StockTraderDbContextFactory : IDbContextFactory<StockTraderDbContext>
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public StockTraderDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public StockTraderDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<StockTraderDbContext> options = new();

            _configureDbContext(options);

            return new StockTraderDbContext(options.Options);
        }
    }
}
