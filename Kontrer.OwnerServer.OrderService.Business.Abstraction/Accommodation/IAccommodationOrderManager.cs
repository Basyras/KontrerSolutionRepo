using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Kontrer.OwnerServer.OrderService.Business.Abstraction.Accommodation
{
    public interface IAccommodationOrderManager
    {
        void CreateOrder(CustomerModel customer, AccommodationBlueprint blueprint, CultureInfo customersCulture, string customerNotes = null);
        void CancelOrder(int orderId, string reason, bool isCanceledByCustomer);
        void EditOrder();
        void GetOrders();

    }
}
