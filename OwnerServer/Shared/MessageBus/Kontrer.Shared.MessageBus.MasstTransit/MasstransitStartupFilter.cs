using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Kontrer.Shared.MessageBus.MasstTransit
{
    public class MassTransitStartupFilter : IStartupFilter
    {
        private readonly IBusControl massTransitBus;

        public MassTransitStartupFilter(IBusControl massTransitBus)
        {
            this.massTransitBus = massTransitBus;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (app) =>
            {
                //var massTransitBus = app.ApplicationServices.GetRequiredService<IBusControl>();

                //var health = massTransitBus.CheckHealth();
                //if(health.Status == BusHealthStatus.Unhealthy) throw new Exception("");
                //var healthStatus = massTransitBus.WaitForHealthStatus(BusHealthStatus.Healthy, TimeSpan.FromSeconds(50)).GetAwaiter().GetResult();
                //if (healthStatus == BusHealthStatus.Unhealthy)
                //    throw new Exception("Message bus is unhealthy! (probablly not running)");

                //RabbitMQ must be running here! or it will hold for ever - not true
                try
                {
                    massTransitBus.Start(new TimeSpan(0, 0, 30));
                    //massTransitBus.CheckHealth();
                }
                catch (Exception ex)
                {
                    //Could not connect to RabbitMQ?
                    throw new Exception("MassTransit could not connect to a cus", ex);
                }

                next(app);
            };
        }
    }
}