using Kontrer.OwnerServer.IdGeneratorService.Application;
using Kontrer.OwnerServer.IdGeneratorService.Infrastructure.EntityFramework;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Kontrer.Shared.Repositories.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            //MicroserviceBootstrapper.CreateBuilder<Startup, CreateNewIdCommandHandler>(args)
            //    .Back()
            //    .Build()
            //    .Run();

            var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);

            builder.AddMessageBus()
                .RegisterRequestHandlers(typeof(CreateNewIdCommandHandler).Assembly)
                .AddMassTransitProvider(typeof(CreateNewIdCommandHandler).Assembly);

            await builder.Back()
                 .Build()
                 .RunAsync();
        }
    }
}