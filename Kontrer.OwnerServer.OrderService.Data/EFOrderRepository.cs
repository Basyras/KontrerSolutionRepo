﻿using Kontrer.OwnerServer.OrderService.Data.Abstraction;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kontrer.OwnerServer.OrderService.Data
{
    public class EFOrderRepository : IAccommodaionOrderRepository
    {
        public void AddOrder(AccommodationOrder order)
        {
            throw new NotImplementedException();
        }

        public void EditOrder(AccommodationOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, AccommodationOrder>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public void RemoveOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
