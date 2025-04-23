
using Domain.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peresistence;
using Services;
using Services.Abstractions;
using Shared.ErrorModel;
using Store.Application.Api.Extensions;
using Store.Application.Api.Middlewares;

namespace Store.Application.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

             await app.ConfigurationMiddleWares();


            app.Run();
        }
    }
}
