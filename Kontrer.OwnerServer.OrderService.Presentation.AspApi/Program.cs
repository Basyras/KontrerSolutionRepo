using Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Basyc.MicroService.Asp.Bootstrapper;
using Basyc.Repositories.EF;

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