using Basyc.Asp;
using Kontrer.OwnerServer.PricingService.Application;
using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework;
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

namespace Kontrer.OwnerServer.PricingService.Presentation.AspApi
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

          services.AddPricingService()
                .AddEFRepository(Configuration.GetConnectionString("DefaultConnection"))
                .AddBlueprintEditor<AddCustomerHistoryDiscountEditor>()
                .AddPricer<AccommodationApplyDiscountPricer>()
                .AddPricer<AccommodationItemTaxPricer>()
                .AddPricer<AccommodationBasicCostPricer>();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
