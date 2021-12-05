using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Microsoft.Extensions.Logging;
using Basyc.Repositories.EF;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework
{
    public class EFAccommodationOrderRepository : EFInstantCrudRepositoryBase<AccommodationOrderEntity, int>, IAccommodationOrderRepository
    {
        public EFAccommodationOrderRepository(DbContext dbContext, ILogger<EFAccommodationOrderRepository> logger) : base(dbContext, entity => entity.Id, logger)
        {
        }

        public async Task<Dictionary<int, AccommodationOrderEntity>> GetCompletedAsync()
        {
            var dic = await this.dbContext.Set<AccommodationOrderEntity>().Where(x => x.State == Domain.Orders.OrderStates.Completed).ToDictionaryAsync(x => x.Id);
            return dic;
        }

        public async Task<Dictionary<int, AccommodationOrderEntity>> GetNewOrdersAsync()
        {
            var dic = await dbContext.Set<AccommodationOrderEntity>()
                .Where(x => x.State == Domain.Orders.OrderStates.New)
                .ToDictionaryAsync(x => x.Id);
            return dic;
        }

        public async Task<Dictionary<int, AccommodationOrderEntity>> GetProcessedAsync()
        {
            var dic = await dbContext.Set<AccommodationOrderEntity>()
                 .Where(x => x.State == Domain.Orders.OrderStates.Processed)
                 .ToDictionaryAsync(x => x.Id);
            return dic;
        }
    }
}