using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Basyc.MessageBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandBox.AspApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IMessageBusManager messageBusManager;

        public DebugController(IMessageBusManager messageBusManager)
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