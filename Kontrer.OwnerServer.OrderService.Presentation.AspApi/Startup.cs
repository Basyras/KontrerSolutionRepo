using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework;
using Kontrer.OwnerServer.Shared.Asp;
using Kontrer.Shared.MessageBus.MasstTransit;
using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements;

namespace Kontrer.OwnerServer.OrderService.Presentation.AspApi
{
    public class Startup : IStartupClass
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=OrderServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var connstring = "Server=(localdb)\\mssqllocaldb;Database=OrderServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            //services.AddSingleton<DbContext, EFAccommodationOrderRepository>();

            services.AddDbContext<DbContext, OrderServiceDbContext>(options =>
                     options.UseSqlServer(debugConnectionString));

            services.AddScoped<IAccommodationOrderRepository, EFAccommodationOrderRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbContext>();
                db.Database.Migrate();

                var repo = scope.ServiceProvider.GetService<IAccommodationOrderRepository>();
                var ids = repo.GetAllAsync().GetAwaiter().GetResult().Select(x => x.Value.Id).ToList();
                foreach (var id in ids)
                {
                    repo.RemoveAsync(id).GetAwaiter().GetResult();
                }
                var req = new AccommodationRequirement();
                var room = new RoomRequirement();
                room.People.Add(new PersonBlueprint());
                room.People.Add(new PersonBlueprint());
                room.People.Add(new PersonBlueprint());
                req.Rooms.Add(room);
                req.Rooms.Add(new RoomRequirement());
                req.Rooms.Add(new RoomRequirement());
                repo.AddAsync(new AccommodationOrderEntity(0, 1, req, DateTime.Now, "asd", "xxx") { State = Domain.Orders.OrderStates.New }).GetAwaiter().GetResult();
            }
        }
    }
};