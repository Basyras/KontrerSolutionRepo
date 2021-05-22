using Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi.Actors;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Dapr;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MicroserviceBootstrapper.CreateMicroserviceHostBuilder<Startup>(args);
        }

     
    }
}
