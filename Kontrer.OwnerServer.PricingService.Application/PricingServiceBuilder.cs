using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application
{
    public class PricingServiceBuilder
    {
        private readonly IServiceCollection services;

        public PricingServiceBuilder(IServiceCollection services)
        {
            this.services = services;
        }
        public PricingServiceBuilder AddRepository<TSettingsRepisotory>() where TSettingsRepisotory : class, ISettingsRepository
        {
            services.AddSingleton<ISettingsRepository,TSettingsRepisotory>();
            return this;
        }

        public PricingServiceBuilder AddBlueprintEditor<TAccommodationBlueprintEditor>() where TAccommodationBlueprintEditor : class, IAccommodationBlueprintEditor
        {
            services.AddSingleton<IAccommodationBlueprintEditor, TAccommodationBlueprintEditor>();
            return this;
        }

        public PricingServiceBuilder AddPricer<TAccommodationPricer>() where TAccommodationPricer : class, IAccommodationPricer
        {
            services.AddSingleton<TAccommodationPricer, TAccommodationPricer>();
            return this;
        }



    }
}
