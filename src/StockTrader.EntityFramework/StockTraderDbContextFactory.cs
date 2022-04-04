using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StockTrader.EntityFramework
{
    public class StockTraderDbContextFactory : IDesignTimeDbContextFactory<StockTraderDbContext>
    {
        public StockTraderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockTraderDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=StockTrader;User ID=postgres;Password=admin;");

            return new StockTraderDbContext(optionsBuilder.Options);
        }
    }
}