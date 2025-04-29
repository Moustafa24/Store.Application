using Domain.Contracts;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using peresistence;
using peresistence.Identity;
using Services;
using Shared;
using Shared.ErrorModel;
using Store.Application.Api.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Store.Application.Api.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServices (this IServiceCollection services , IConfiguration configuration)
        {

            services.AddBuiltInServices();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerServices();
            services.ConfgureServices();


           services.AddInfrastructureServices(configuration);
            services.AddIdentityServices();

           services.AddApplicationServices(configuration);

            services.ConfigureJwtServices(configuration);
     

            return services;
        }



        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();


            return services;
        }

        private static IServiceCollection  ConfigureJwtServices(this IServiceCollection services , IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,


                    ValidIssuer = JwtOptions.Issuer,
                    ValidAudience = JwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey))
                };
            });




            return services;
        }


        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();


            return services;
        }


        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;
        }

        private static IServiceCollection ConfgureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                   .Select(m => new ValidationError()
                   {
                       Field = m.Key,
                       Errors = m.Value.Errors.Select(error => error.ErrorMessage)
                   });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            }

           );


            return services;
        }

        public static async Task<WebApplication> ConfigurationMiddleWares(this WebApplication app)
        {

            #region Seeding
            await app.InitalizeDatabaseAsync();


            #endregion

           app.UseGlobalErrorHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;

        }

        private static async Task<WebApplication> InitalizeDatabaseAsync(this WebApplication app)
        {

            #region Seeding

            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // ASK CLR Create Object  
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            #endregion

           

            return app;

        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();


            return app;

        }

    }


}
