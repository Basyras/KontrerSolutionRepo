using Basyc.MessageBus.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
    public record CustomerCreatedEvent(CustomerEntity CreatedCustomer) : IEventMessage;
}
