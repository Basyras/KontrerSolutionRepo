using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Presentation.BlazorLibrary;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServicesBusMangerExtensions
    {
        public static BusManagerBuilder AddBlazorMessageBus(this IServiceCollection services)
        {
            services.AddMudServices();
            services.AddSingleton<BusManagerJSInterop>();
            return services.AddMessageManager();
        }
    }
}