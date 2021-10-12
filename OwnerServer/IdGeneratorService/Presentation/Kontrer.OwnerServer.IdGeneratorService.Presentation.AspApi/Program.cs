using Kontrer.OwnerServer.IdGeneratorService.Application;
using Kontrer.OwnerServer.IdGeneratorService.Infrastructure.EntityFramework;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Data.EF;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Kontrer.Shared.Repositories.EF;
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
        public static void Main(string[] args)
        {
            MicroserviceBootstrapper.CreateMicroserviceHostBuilder<Startup, CreateNewIdCommandHandler>(args)
                .Build()
                .MigrateDatabase<IdGeneratorDbContext>()
                .Run();
        }
    }
}