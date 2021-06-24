using Kontrer.OwnerServer.OrderService.Dtos.Models;
using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Dtos.Requests
{
    public class CreateNewOrderCommand : IRequest<AccommodationOrder>
    {
        public CreateNewOrderCommand(int customerId, AccommodationBlueprint blueprint, CultureInfo orderCulture)
        {
            CustomerId = customerId;
            Blueprint = blueprint;
            OrderCulture = orderCulture;
        }

        public int CustomerId { get; }
        public AccommodationBlueprint Blueprint { get; }
        public CultureInfo OrderCulture { get; }
    }
}