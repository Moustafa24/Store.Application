using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Shared;

namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(AssemblyRefrence).Assembly);
            services.AddScoped<IServiceManager, ServicesManager>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }

    }
}
