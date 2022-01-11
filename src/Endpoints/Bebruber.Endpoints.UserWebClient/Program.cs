using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bebruber.Endpoints.UserWebClient.Interfaces;
using Bebruber.Endpoints.UserWebClient.Services;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using FisSst.BlazorMaps.DependencyInjection;

namespace Bebruber.Endpoints.UserWebClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazorise( options =>
                                           {
                                               options.ChangeTextOnKeyPress = true;
                                           } )
                   .AddBootstrapProviders()
                   .AddFontAwesomeIcons();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddBlazorLeafletMaps();

            await builder.Build().RunAsync();
        }
    }
}