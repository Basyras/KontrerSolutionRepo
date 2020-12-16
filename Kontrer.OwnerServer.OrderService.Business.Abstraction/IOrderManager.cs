using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.OwnerServer.OrderService.Business.Abstraction
{
    interface IOrderManager
    {
        void CreateOrder();
        void RemoveOrder();
        void EditOrder();
        void GetOrders();

        

    }
}
