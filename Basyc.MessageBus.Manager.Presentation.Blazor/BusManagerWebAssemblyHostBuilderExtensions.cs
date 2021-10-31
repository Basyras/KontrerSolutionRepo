using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Presentation.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BusManagerWebAssemblyHostBuilderExtensions
    {
        public static BusManagerBuilder AddBusManager(this WebAssemblyHostBuilder blazorBuilder)
        {
            blazorBuilder.RootComponents.Add<App>("#app");
            blazorBuilder.Services.AddMudServices();
            blazorBuilder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(blazorBuilder.HostEnvironment.BaseAddress) });
            return new BusManagerBuilder(blazorBuilder.Services);
        }
    }
}