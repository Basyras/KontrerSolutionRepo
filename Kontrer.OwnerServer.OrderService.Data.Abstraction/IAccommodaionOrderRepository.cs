using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Data.Abstraction
{
   public interface IAccommodaionOrderRepository
    {
        void AddOrder(AccommodationOrder order);
        void RemoveOrder(int orderId);
        void EditOrder(AccommodationOrder order);
        Task<Dictionary<string,AccommodationOrder>> GetOrdersAsync();
        Task<AccommodationOrder> GetOrderAsync(int orderId);

        Task CommitAsync();
    }
}
