using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AzureStaticWebApps.Blazor.Authentication;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;

            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress), DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json")}}});

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddStaticWebAppsAuthentication();
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}