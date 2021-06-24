using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.Shared.Repositories.EF;
using Kontrer.OwnerServer.OrderService.Application.Interfaces;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework
{
    public class EFAccommodationOrderRepository : EFInstantCrudRepositoryBase<AccommodationOrderEntity, int, Domain.Orders.AccommodationOrders.AccommodationOrderEntity, int>, IAccommodationOrderRepository
    {
        public EFAccommodationOrderRepository(DbContext dbContext) : base(dbContext, entity => entity.OrderId)
        {
        }

        public Task<Dictionary<int, Domain.Orders.AccommodationOrders.AccommodationOrderEntity>> GetCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, Domain.Orders.AccommodationOrders.AccommodationOrderEntity>> GetNewOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, Domain.Orders.AccommodationOrders.AccommodationOrderEntity>> GetProcessedAsync()
        {
            throw new NotImplementedException();
        }

        protected override int GetModelId(Domain.Orders.AccommodationOrders.AccommodationOrderEntity model)
        {
            return model.Id;
        }

        protected override void SetEntityId(int id, AccommodationOrderEntity entity)
        {
            entity.OrderId = id;
        }

        protected override void SetModelId(int id, Domain.Orders.AccommodationOrders.AccommodationOrderEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override AccommodationOrderEntity ToEntity(Domain.Orders.AccommodationOrders.AccommodationOrderEntity model)
        {
            throw new NotImplementedException();
        }

        protected override Domain.Orders.AccommodationOrders.AccommodationOrderEntity ToModel(AccommodationOrderEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}