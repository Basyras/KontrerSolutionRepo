using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Dtos.Models
{
    public class AccommodationOrder : GenericOrderModel<AccommodationBlueprint>
    {
        public AccommodationOrder() : base(default, default, default, default, default, default, default, default)
        {
        }

        public AccommodationOrder(int orderId, int customerId, AccommodationBlueprint blueprint, DateTime creationDate, OrderStates state, CultureInfo culture, string customerPrivateNotes = null, string ownerPrivateNotes = null)
            : base(orderId, customerId, blueprint, creationDate, state, culture, customerPrivateNotes, ownerPrivateNotes)
        {
        }
    }
}