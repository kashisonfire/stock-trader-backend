using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockTrader.Domain.Models;
using StockTrader.EntityFramework;
using System;

namespace StockTrader.API.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {
                // Get provider name and construct db connection
                string providerNameString = context.Configuration.GetConnectionString(nameof(ProviderName));
                ProviderName providerName = (ProviderName)Enum.Parse(typeof(ProviderName), providerNameString);
                string connectionString = context.Configuration.GetConnectionString(providerNameString + "ConnectionString");

                // Options builder based on provider
                Action<DbContextOptionsBuilder> configureDbContext = providerName switch
                {
                    ProviderName.Npgsql => o =>
                    {
                        o.UseNpgsql(connectionString)
                            .LogTo(Console.WriteLine)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
                    }
                    ,

                    _ => throw new ArgumentException(nameof(providerName)),
                };

                // Context factory for database services
                services.AddDbContextFactory<StockTraderDbContext>(configureDbContext);
            });

            return host;
        }
    }
}