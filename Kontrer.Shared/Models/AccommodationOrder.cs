using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models
{
    public class AccommodationOrder : GenericOrderModel<AccommodationBlueprint>
    {
        public AccommodationOrder(int orderId, int customerId, AccommodationBlueprint blueprint, DateTime creationDate, OrderStates state,CultureInfo culture, string customerPrivateNotes = null, string ownerPrivateNotes = null)
            : base(orderId,customerId, blueprint, creationDate, state,culture, customerPrivateNotes, ownerPrivateNotes)
        {
            
        }
    }
}
