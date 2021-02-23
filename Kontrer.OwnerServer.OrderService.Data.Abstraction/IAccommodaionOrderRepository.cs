using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Data.Abstraction
{
   public interface IAccommodaionOrderRepository : ICrudRepository<AccommodationOrder,int>
    { 

        Task CommitAsync();
    }
}
