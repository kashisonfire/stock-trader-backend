using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StockTrader.API.Auth;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.EntityFramework;
using StockTrader.EntityFramework.Services;
using StockTrader.Logger;
using StockTrader.Utilities.PasswordHasher;
using System;

namespace StockTrader.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Container</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Get provider name and construct db connection
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string providerNameString = configuration.GetConnectionString(nameof(ProviderName));
            ProviderName providerName = (ProviderName)Enum.Parse(typeof(ProviderName), providerNameString);
            string connectionString = configuration.GetConnectionString(providerNameString + "ConnectionString");

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
            services.AddDbContext<StockTraderDbContext>(configureDbContext);
            services.AddSingleton(new StockTraderDbContextFactory(configureDbContext));

            // Add db services
            services.AddSingleton(typeof(IDataService<>), typeof(GenericDataService<>));
            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            // Authentication
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ILogger, Logger.Logger>();
            services.AddSingleton<IAuthenticator, Authenticator>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockTrader.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockTrader.API v1"));
            }

            // migrate database if needed
            StockTraderDbContextFactory contextFactory = app.ApplicationServices.GetRequiredService<StockTraderDbContextFactory>();
            using (StockTraderDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }

            // configure logger
            app.ApplicationServices.GetRequiredService<ILogger>().Configure();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}