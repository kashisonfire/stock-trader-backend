using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace StockTrader.API.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration(context =>
            {
                context.AddJsonFile("appsettings.json");
                context.AddEnvironmentVariables();
            });

            return host;
        }
    }
}