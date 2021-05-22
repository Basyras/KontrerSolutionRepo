using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
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
        public readonly IServiceCollection Services;

        public PricingServiceBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        public PricingServiceBuilder AddRepository<TSettingsRepisotory>() where TSettingsRepisotory : class, ISettingsRepository
        {
            Services.AddSingleton<ISettingsRepository,TSettingsRepisotory>();
            return this;
        }

        public PricingServiceBuilder AddBlueprintEditor<TAccommodationBlueprintEditor>() where TAccommodationBlueprintEditor : class, IAccommodationBlueprintEditor
        {
            Services.AddSingleton<IAccommodationBlueprintEditor, TAccommodationBlueprintEditor>();
            return this;
        }

        public PricingServiceBuilder AddPricer<TAccommodationPricer>() where TAccommodationPricer : class, IAccommodationPricer
        {
            Services.AddSingleton<IAccommodationPricer, TAccommodationPricer>();
            return this;
        }   
        
        public PricingServiceBuilder AddUnitOfWorkFactory<TUnitOfWorkFactory>() where TUnitOfWorkFactory : class, IUnitOfWorkFactory<ISettingsUnitOfWork>
        {
            Services.AddSingleton<IUnitOfWorkFactory<ISettingsUnitOfWork>, TUnitOfWorkFactory>();
            return this;
        }



    }
}
