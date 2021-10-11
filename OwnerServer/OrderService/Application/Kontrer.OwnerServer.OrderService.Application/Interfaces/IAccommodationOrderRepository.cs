﻿using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Interfaces
{
    public interface IAccommodationOrderRepository : IAsyncInstantCrudRepository<AccommodationOrderEntity, int>
    {
    }
}