using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StockTrader.Domain.Models;
using StockTrader.EntityFramework;
using System;
using System.Configuration;

namespace StockTrader.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            string providerNameString = ConfigurationManager.AppSettings[nameof(ProviderName)] ?? "";
            ProviderName providerName = (ProviderName)Enum.Parse(typeof(ProviderName), providerNameString);
            string connectionString = ConfigurationManager.ConnectionStrings[providerName.ToString()].ConnectionString;

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
            services.AddDbContextFactory<StockTraderDbContext, StockTraderDbContextFactory>(configureDbContext);

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

            StockTraderDbContextFactory contextFactory = app.ApplicationServices.GetRequiredService<StockTraderDbContextFactory>();
            using (StockTraderDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }

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