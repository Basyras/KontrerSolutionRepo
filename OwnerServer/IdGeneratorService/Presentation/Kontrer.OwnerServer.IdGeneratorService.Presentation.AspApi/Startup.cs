using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Consumers;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Data.EF;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
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
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi
{
    public class Startup : IStartupClass
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext, IdGeneratorServiceDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));               
            }, ServiceLifetime.Singleton);

            services.AddIdGenerator();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var dbContext = app.ApplicationServices.GetRequiredService<DbContext>();
            dbContext.Database.Migrate();

            //var busManager = app.ApplicationServices.GetRequiredService<IMessageBusManager>();
            //busManager.RegisterConsumer<AccommodationIdCreatedConsumer>();


        }
    }
}
