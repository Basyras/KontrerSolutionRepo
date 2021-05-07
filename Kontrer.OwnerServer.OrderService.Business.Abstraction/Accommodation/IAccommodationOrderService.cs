using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Business.Abstraction.Accommodation
{
    public interface IAccommodationOrderService
    {
        Task<AccommodationOrder> CreateOrderAsync(int customerId, AccommodationBlueprint blueprint, CultureInfo customersCulture);
        Task CancelOrderAsync(int orderId, string reason, bool isCanceledByCustomer);
        Task EditOrderAsync(int orderId, AccommodationBlueprint accommodationBlueprint);
        Task<List<AccommodationOrder>> GetOrders();

    }
}
