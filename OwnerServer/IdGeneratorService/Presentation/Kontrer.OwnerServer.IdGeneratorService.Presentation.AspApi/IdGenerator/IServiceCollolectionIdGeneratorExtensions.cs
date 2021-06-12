using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Abstraction.Data;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Data.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator
{
    public static class IServiceCollolectionIdGeneratorExtensions
    {
        public static IServiceCollection AddIdGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IIdGeneratorManager, IdGeneratorManager>();
            services.AddSingleton<IIdGeneratorStorage, EFIdGeneratorStorage>();            

            return services;
        }
    }
}
