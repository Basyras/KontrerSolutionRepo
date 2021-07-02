using Kontrer.OwnerServer.Shared.MessageBus.RequestResponse;
using Kontrer.Shared.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Kontrer.OwnerServer.Shared.MicroService.MessageBus.Asp
{
    public static class IServiceCollectionMessageBusExtensions
    {
        public static IWebHostBuilder RegisterHandlersToDI(this IWebHostBuilder webBuilder, Assembly handlersAssembly)
        {
            webBuilder.ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
            {
                services.Scan(scan =>
                scan.FromAssemblies(handlersAssembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>))).As(handler => new Type[1] { typeof(IRequestHandler<>).MakeGenericType(GenericHelper.GetGenericTypeRecursive(handler, typeof(IRequestHandler<>))) }).WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>))).As(handler => new Type[1] { typeof(IRequestHandler<,>).MakeGenericType(GenericHelper.GetGenericTypeRecursive(handler, typeof(IRequestHandler<,>))) }).WithScopedLifetime()
                );
            });

            return webBuilder;
        }
    }
}