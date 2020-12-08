using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.AspNetCore;
using Kontrer.OwnerServer.Shared.Actors.PdfCreator;
using Kontrer.OwnerServer.PdfCreatorService.Actors;
using Dapr.Actors.Runtime;

namespace Kontrer.OwnerServer.PdfCreatorService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {                    
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseActors(actorOptions =>
                    {                        
                        actorOptions.RegisterActor<TestActor>();
                        //actorOptions.ConfigureActorSettings(a =>
                        //{
                        //    a.ActorIdleTimeout = TimeSpan.FromMinutes(70);
                        //    a.ActorScanInterval = TimeSpan.FromSeconds(35);
                        //    a.DrainOngoingCallTimeout = TimeSpan.FromSeconds(35);
                        //    a.DrainRebalancedActors = true;
                        //});
                    });
                });


      
    }
}
