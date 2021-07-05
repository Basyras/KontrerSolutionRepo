using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Kontrer.Shared.MessageBus.Asp
{
    //public static class IServiceCollectionMessageBusExtensions
    //{
    //    public static IWebHostBuilder RegisterHandlersToDI(this IWebHostBuilder webBuilder, Assembly handlersAssembly)
    //    {
    //        webBuilder.ConfigureServices((context, services) =>
    //        {
    //            services.Scan(scan =>
    //            scan.FromAssemblies(handlersAssembly)
    //            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>))).As(handler => new Type[1] { typeof(IRequestHandler<>).MakeGenericType(GenericHelper.GetGenericTypeRecursive(handler, typeof(IRequestHandler<>))) }).WithScopedLifetime()
    //            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>))).As(handler => new Type[1] { typeof(IRequestHandler<,>).MakeGenericType(GenericHelper.GetGenericTypeRecursive(handler, typeof(IRequestHandler<,>))) }).WithScopedLifetime()
    //            );
    //        });

    //        return webBuilder;
    //    }
    //}
}