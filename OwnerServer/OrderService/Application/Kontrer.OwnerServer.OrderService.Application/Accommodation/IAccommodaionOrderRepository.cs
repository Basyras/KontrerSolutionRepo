using Kontrer.OwnerServer.OrderService.Client.Models;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.Abstraction
{
    public interface IAccommodaionOrderRepository : IInstantCrudRepository<AccommodationOrder, int>
    {
        Task<Dictionary<int, AccommodationOrder>> GetNewOrdersAsync();

        Task<Dictionary<int, AccommodationOrder>> GetProcessedAsync();

        Task<Dictionary<int, AccommodationOrder>> GetCompletedAsync();
    }
}