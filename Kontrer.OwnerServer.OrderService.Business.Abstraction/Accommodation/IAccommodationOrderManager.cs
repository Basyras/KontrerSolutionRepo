using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Business.Abstraction.Accommodation
{
    public interface IAccommodationOrderManager
    {
        Task<AccommodationOrder> CreateOrder(int customerId, AccommodationBlueprint blueprint, CultureInfo customersCulture);
        void CancelOrder(int orderId, string reason, bool isCanceledByCustomer);
        void EditOrder();
        Task<List<AccommodationOrder>> GetOrders();

    }
}
