using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.OwnerServer.OrderService.Data.Abstraction
{
   public interface IOrdersRepository
    {
        void CreateOrder();
        void RemoveOrder();
        void EditOrder();
        void GetOrders();
    }
}
