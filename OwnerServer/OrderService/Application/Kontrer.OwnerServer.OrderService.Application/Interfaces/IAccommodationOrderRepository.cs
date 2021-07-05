using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Interfaces
{
    public interface IAccommodationOrderRepository : IInstantCrudRepository<AccommodationOrderEntity, int>
    {
        Task<Dictionary<int, AccommodationOrderEntity>> GetNewOrdersAsync();

        Task<Dictionary<int, AccommodationOrderEntity>> GetProcessedAsync();

        Task<Dictionary<int, AccommodationOrderEntity>> GetCompletedAsync();
    }
}