﻿using Kontrer.Shared.DomainDrivenDesign.Application;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Asp
{
    [Obsolete("command are register via masstransit instead of endpoints")]
    public class AspMessageBusStartupFilter<TCommand> : IStartupFilter
    {
        private static readonly Type[] commandIntefaceTypes;
        private static readonly Type[] commandHandlerInterfacesTypes;
        private static readonly Assembly commandsAssembly;

        static AspMessageBusStartupFilter()
        {
            commandIntefaceTypes = new Type[] { typeof(ICommand), typeof(IRequest<>) };
            commandHandlerInterfacesTypes = new Type[] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };
            commandsAssembly = typeof(TCommand).Assembly;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (app) =>
            {
                Configure(app);
                next(app);
            };
        }

        private static void Configure(IApplicationBuilder app)
        {
            app.UseEndpoints(endpointBuilder =>
            {
                RegisterCommandsAndHandlers(endpointBuilder, app.ApplicationServices, commandsAssembly);
            });
        }

        private static void RegisterCommandsAndHandlers(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider services, Assembly commandsAssembly)
        {
            var commandAssemblyTypes = commandsAssembly.GetTypes();

            var commandTypes = commandAssemblyTypes.Where(type => commandIntefaceTypes.Any(interfaceType => type.IsAssignableFrom(interfaceType)));

            var commandHandlers = commandAssemblyTypes.Where(type => commandHandlerInterfacesTypes.Any(handlerType => type.IsAssignableFrom(handlerType))).Select(x => new { HandlerType = x, CommandType = x.GenericTypeArguments[0] });

            foreach (var handler in commandHandlers)
            {
                var commandType = commandTypes.First(x => handler.CommandType == x);
                var handlerInstance = services.GetService(handler.HandlerType);

                endpointRouteBuilder.MapPost($"/{commandType.Name}", async context =>
                {
                    await context.Response.WriteAsync($"Command endpoit started. Command: '{commandType.Name}'");
                    var task = (Task)handler.HandlerType.GetMethod(nameof(IRequestHandler<ICommand>.Handle)).Invoke(handlerInstance, null);
                    await task;
                });
            }
        }
    }
}