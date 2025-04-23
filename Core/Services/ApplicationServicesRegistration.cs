using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssemblyRefrence).Assembly);
            services.AddScoped<IServiceManager, ServicesManager>();
            return services;
        }

    }
}
