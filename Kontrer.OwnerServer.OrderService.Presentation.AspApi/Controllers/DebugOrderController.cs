using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrders;
using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using Kontrer.Shared.MessageBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugOrderController : ControllerBase
    {
        private readonly IMessageBusManager messageBusManager;
        private readonly IAccommodationOrderRepository repo;

        public DebugOrderController(IMessageBusManager messageBusManager, IAccommodationOrderRepository repo)
        {
            this.messageBusManager = messageBusManager;
            this.repo = repo;
        }

        [HttpPost]
        public async Task CreateOrder()
        {
            //var result = await messageBusManager.RequestAsync<CreateAccommodationOrderCommand, CreateAccommodationOrderResponse>(new(orderId, null));
            await repo.AddAsync(new AccommodationOrderEntity(0, 0, null, default, "", ""));
        }

        [HttpDelete]
        public async Task CancelOrder()
        {
            //var resilt = new CancelAccommodationOrderCommand(0, "", false);
        }
    }
}