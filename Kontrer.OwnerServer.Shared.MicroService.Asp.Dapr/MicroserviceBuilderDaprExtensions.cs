using Dapr.Actors.AspNetCore;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Dapr
{
    public static class MicroserviceBuilderDaprExtensions
    {
        public static MicroserviceBuilder AddDaprProvider(this MicroserviceBuilder builder)
        {
            var provider = new DaprMicroserviceProvider(builder.WebBuilder);
            builder.AddProvider(provider);            
            return builder;
        }
    }
}
