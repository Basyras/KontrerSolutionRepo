using Kontrer.OwnerServer.PricingService.Application;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public static class PricingServiceInfrastructureIServiceCollectionExtensions
    {
        public static PricingServiceBuilder AddEFRepository(this PricingServiceBuilder builder, string connectionString)
        {
            //builder.AddRepository<EfPricingSettingsRepository>();
            builder.Services.AddScoped<ISettingsRepository, EfPricingSettingsRepository>();
            //builder.AddUnitOfWorkFactory<EfPricingSettingsUnitOfWorkFactory>();
            builder.Services.AddScoped<IUnitOfWorkFactory<ISettingsUnitOfWork>, EfPricingSettingsUnitOfWorkFactory>();
            builder.Services.AddDbContext<PricingServiceDbContext>(options => options.UseSqlServer(connectionString));
            return builder;
        }
    }
}
