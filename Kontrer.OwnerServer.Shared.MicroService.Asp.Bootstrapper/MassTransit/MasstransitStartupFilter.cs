using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MassTransit
{
    public class MasstransitStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {

            return (IApplicationBuilder app) =>
            {
                var massTransitBus = app.ApplicationServices.GetRequiredService<IBusControl>();
                //var health = massTransitBus.CheckHealth();
                //if(health.Status == BusHealthStatus.Unhealthy) throw new Exception("");
                var healthStatus = massTransitBus.WaitForHealthStatus( BusHealthStatus.Healthy,TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                if (healthStatus == BusHealthStatus.Unhealthy) throw new Exception("Message bus is unhealthy! (probablly not running)");

                //RabbitMQ must be running here! or it will hold for ever
                massTransitBus.Start();

                next(app);
            };

        }


    }
}