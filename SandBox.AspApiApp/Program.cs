using Basyc.MessageBus.Client.MasstTransit;
using Basyc.MicroService.Asp.Bootstrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SandBox.AspApiApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);
            builder.AddMessageBus()
                     .AddMassTransitClient();

            builder.Back().Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.ConfigureMessageBusServices<Startup>(Assembly.GetEntryAssembly());
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}