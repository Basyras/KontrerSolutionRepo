using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.RazorLight;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.SelectPdf;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus;
using Kontrer.Shared.Localizator;
using Kontrer.Shared.Localizator.EF;
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
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Initialization;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Basyc.Asp;
using Basyc.Localizator.Abstraction.Initialization;
using Basyc.Localizator.Infrastructure.EF.Initialization;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi
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
            services.AddPdfBuilder();
            services.AddLocalizator().AddEfStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}