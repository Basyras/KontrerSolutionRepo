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
                repo.AddAsync(new AccommodationOrderEntity(0, 1, new(), DateTime.Now, "asd", "xxx") { State = Domain.Orders.OrderStates.New }).GetAwaiter().GetResult();
            }
        }
    }

    public class DummyRepo : IAccommodationOrderRepository
    {
        public Task<AccommodationOrderEntity> AddAsync(AccommodationOrderEntity model)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, AccommodationOrderEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccommodationOrderEntity> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, AccommodationOrderEntity>> GetCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, AccommodationOrderEntity>> GetNewOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, AccommodationOrderEntity>> GetProcessedAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccommodationOrderEntity> TryGetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccommodationOrderEntity> UpdateAsync(AccommodationOrderEntity model)
        {
            throw new NotImplementedException();
        }
    }
};