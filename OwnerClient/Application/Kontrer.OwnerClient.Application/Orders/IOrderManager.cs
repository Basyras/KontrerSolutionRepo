using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Orders
{
    public interface IOrderManager
    {
        ValueTask<List<OrderViewModel>> GetOrders();

        Task DeleteOrder(int orderId);

        ValueTask<OrderViewModel> CreateOrder(int customerId);
    }
}