using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Presentation.AspApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MicroserviceBootstrapper.CreateMicroserviceHostBuilder<Startup>(args).Build().Run();
        }

       
    }
}
