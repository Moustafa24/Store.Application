
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using peresistence;
using Services;
using Services.Abstractions;
using AssemblyMapping = Services.AssemblyRefrence;
namespace Store.Application.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                // options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);  
                options.UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection"));
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(AssemblyMapping).Assembly);
            builder.Services.AddScoped<IServiceManager, ServicesManager>();

            var app = builder.Build();


            #region Seeding

            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // ASK CLR Create Object  
            await  dbInitializer.InitializeAsync();


            #endregion



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
