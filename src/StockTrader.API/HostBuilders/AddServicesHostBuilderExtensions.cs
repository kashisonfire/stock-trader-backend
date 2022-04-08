using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockTrader.API.Authentication;
using StockTrader.Domain.Services;
using StockTrader.Domain.Services.Authentication;
using StockTrader.EntityFramework.Services;
using StockTrader.EntityFramework.Services.Authentication;
using StockTrader.Logger;
using StockTrader.Utilities.PasswordHasher;

namespace StockTrader.API.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton(typeof(IDataService<>), typeof(GenericDataService<>));
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IAccountService, AccountService>();

                services.AddSingleton<IPasswordHasher, PasswordHasher>();
                services.AddSingleton<ILogger, Logger.Logger>();
                services.AddSingleton<IAuthenticator, Authenticator>();
            });

            return host;
        }
    }
}