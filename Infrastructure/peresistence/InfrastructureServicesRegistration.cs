using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using peresistence.Identity;
using peresistence.Repositeries;
using StackExchange.Redis;

namespace peresistence
{
    public static class InfrastructureServicesRegistration
    {
        public static  IServiceCollection AddInfrastructureServices (this IServiceCollection services , IConfiguration configuration)
        {

            services.AddDbContext<StoreDbContext>(options =>
            {
                // options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);  
                options.UseSqlServer(configuration.GetConnectionString(name: "DefaultConnection"));
            });
           
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(name: "IdentityConnection"));
            
            });
           
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            
            services.AddSingleton<IConnectionMultiplexer>((ServiceProvider)=>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            });

            return services;
        }
    }
}
