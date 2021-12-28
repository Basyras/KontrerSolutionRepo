using Basyc.MessageBus.Client.MasstTransit;
using Basyc.MicroService.Asp.Bootstrapper;
using Kontrer.OwnerServer.IdGeneratorService.Application;
using Kontrer.OwnerServer.IdGeneratorService.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);

            builder.AddMessageBus()
                .AddMassTransitClient();

            await builder.Back()
                 .Build()
                 .RunAsync();
        }
    }
}