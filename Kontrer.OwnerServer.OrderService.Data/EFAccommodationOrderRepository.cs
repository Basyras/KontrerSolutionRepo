using Kontrer.OwnerServer.OrderService.Data.Abstraction;
using Kontrer.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Shared.Data.EF.Repositories;
using Kontrer.OwnerServer.OrderService.Data.EntityFramework;

namespace Kontrer.OwnerServer.OrderService.Data
{
    public class EFAccommodationOrderRepository : EFCrudRepositoryBase<AccommodationOrderEntity,AccommodationOrder,int,int>,IAccommodaionOrderRepository
    {
        public EFAccommodationOrderRepository(DbContext dbContext)  : base(dbContext,entity=>entity.OrderId)
        {
            
        }

    

        protected override int GetModelId(AccommodationOrder model)
        {
            return model.OrderId;
        }

        protected override void SetEntityId(int id, AccommodationOrderEntity entity)
        {
            entity.OrderId = id;
        }

        protected override AccommodationOrderEntity ToEntity(AccommodationOrder model)
        {
            throw new NotImplementedException();
        }

        protected override AccommodationOrder ToModel(AccommodationOrderEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
