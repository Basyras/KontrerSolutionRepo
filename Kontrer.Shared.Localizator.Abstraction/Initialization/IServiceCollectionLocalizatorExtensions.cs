using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Localizator.Initialization
{
    public static class IServiceCollectionLocalizatorExtensions
    {
        public static LocalizatorBuilder AddLocalizator(this IServiceCollection services)
        {
            services.AddSingleton<ILocalizationManager, LocalizationManager>();

            return new LocalizatorBuilder(services);


        }
    }
}
