using Kontrer.OwnerServer.OrderService.Application.Accommodation;
using Kontrer.OwnerServer.OrderService.Client.Models;
using Kontrer.OwnerServer.OrderService.Client.Models.Blueprints;
using Kontrer.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AccommodationOrderManager manager;

        public OrderController(AccommodationOrderManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccommodationOrder>>> GetNewOrders()
        {
            var orders = await manager.GetNewAsync();
            return orders;
        }

        [HttpPost]
        public Task CreateOrder(int customerId, AccommodationBlueprint accommodationBlueprint)
        {
            var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
            return manager.CreateOrderAsync(customerId, accommodationBlueprint, culture);
        }

        [HttpPut]
        public Task UpdateOrder(int orderId, AccommodationBlueprint accommodationBlueprint)
        {
            return manager.EditOrderAsync(orderId, accommodationBlueprint);
        }

        [HttpDelete]
        public Task CancelOrder(int orderId, string reason, bool isCanceledByCustomer)
        {
            return manager.CancelOrderAsync(orderId, reason, isCanceledByCustomer);
        }
    }
}