using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.OwnerServer.OrderService.Business.Abstraction
{
    public interface IOrderManager
    {
        void CreateOrder();
        void RemoveOrder();
        void EditOrder();
        List<Order> GetOrders();

        

    }
}
