using Kontrer.OwnerServer.IdGeneratorService.Application.Interfaces;
using Kontrer.OwnerServer.IdGeneratorService.Infrastructure.EntityFramework;
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
            services.AddSingleton<IIdGeneratorRepository, EFIdGeneratorRepository>();

            return services;
        }
    }
}