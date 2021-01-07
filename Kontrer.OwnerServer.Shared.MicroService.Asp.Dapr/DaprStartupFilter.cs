using Dapr;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Dapr
{
    public class DaprStartupFilter : IStartupFilter
    {
        private readonly IMessageBusManager messageBusManager;

        public DaprStartupFilter(IMessageBusManager messageBusManager)
        {
            this.messageBusManager = messageBusManager;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (IApplicationBuilder app) =>
            {
                Configure(app);
                next(app);
            };
        }

        private void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();        
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers().Add(builder=> 
                {
                    var controllerDesc = builder.Metadata.First(x => x.GetType() == typeof(ControllerActionDescriptor)) as ControllerActionDescriptor;                                      
                    builder.Metadata.Add(new TopicAttribute(messageBusManager.BusName, controllerDesc.ActionName));
                });                
                
                foreach (var subs in messageBusManager.BusSubscriptions)
                {
                    
                    //var subEndpoint = subs.RequestType.Name;
                    endpoints.MapPost(subs.Topic, (Microsoft.AspNetCore.Http.RequestDelegate)subs.Handler).WithTopic(messageBusManager.BusName, subs.Topic);
                }

            });
            app.UseCloudEvents();
        }
    }
}