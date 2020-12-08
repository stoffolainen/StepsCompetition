using System;
using BlazorApp.Api.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(BlazorApp.Api.Startup))]
namespace BlazorApp.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString));
        }
    }
}