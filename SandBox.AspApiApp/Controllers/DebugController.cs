using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basyc.MessageBus.Client;

namespace SandBox.AspApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly ITypedMessageBusClient messageBusManager;

        public DebugController(ITypedMessageBusClient messageBusManager)
        {
            this.messageBusManager = messageBusManager;
        }

        [HttpGet]
        public async Task Get()
        {
            //await messageBusManager.SendAsync<CancelAccommodationOrderCommand>(new CancelAccommodationOrderCommand(0, "0", true));
            await messageBusManager.SendAsync<DeleteCustomerCommand>(new DeleteCustomerCommand(1));
        }
    }
}