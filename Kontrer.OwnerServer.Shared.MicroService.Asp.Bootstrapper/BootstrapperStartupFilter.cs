﻿using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public class BootstrapperStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {

            return (IApplicationBuilder app) =>
            {

                var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName + " v1"));
                }

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseAuthorization();

                var massTransitBus = app.ApplicationServices.GetRequiredService<IBusControl>();

                var busHealthService = app.ApplicationServices.GetRequiredService<IBusHealth>();
                //var health = busHealth.CheckHealth();
                //if(health.Status == BusHealthStatus.Unhealthy) throw new Exception("");
                var healthStatus = busHealthService.WaitForHealthStatus( BusHealthStatus.Healthy,TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                if (healthStatus == BusHealthStatus.Unhealthy) throw new Exception("Message bus is unhealthy! (probablly not running)");

                //RabbitMQ must be running here! or it will hold for ever
                massTransitBus.Start();



                next(app);
            };

        }


    }
}