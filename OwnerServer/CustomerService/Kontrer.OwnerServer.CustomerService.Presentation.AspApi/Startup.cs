using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework;
using Kontrer.OwnerServer.Shared.Asp;
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
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi
{
    public class Startup : IStartupClass
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext, CustomerServiceDbContext>(options =>
                     options.UseSqlServer(debugConnectionString));
            services.AddScoped<ICustomerRepository, EFCustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbContext>();
                db.Database.Migrate();

                //var repo = scope.ServiceProvider.GetService<ICustomerRepository>();
                //var ids = repo.GetAllAsync().GetAwaiter().GetResult().Select(x => x.Value.Id).ToList();
                //foreach (var id in ids)
                //{
                //    repo.RemoveAsync(id).GetAwaiter().GetResult();
                //}
                //repo.AddAsync(new CustomerEntity() { FirstName = "Jan", SecondName = "Skrrrr" }).GetAwaiter().GetResult();
                //repo.AddAsync(new CustomerEntity() { FirstName = "Johan", SecondName = "Andreiovic" }).GetAwaiter().GetResult();
            }
        }
    }
}