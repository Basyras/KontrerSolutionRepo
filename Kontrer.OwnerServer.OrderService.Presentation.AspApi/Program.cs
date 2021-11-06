using Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework;
using Kontrer.Shared.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kontrer.OwnerServer.OrderService.Presentation.AspApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);

            builder.AddMessageBus()
                .RegisterBasycRequestHandlers<CreateAccommodationOrderCommandHandler>()
                .AddMassTransitProvider();

            builder
                .Back()
                .MigrateDatabaseOnStart<DbContext>()
            //.MigrateDatabaseOnStart<OrderServiceDbContext>()
                .Build()
                .Run();
        }
    }
}