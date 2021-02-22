using Kontrer.OwnerServer.OrderService.Business.Abstraction;
using Kontrer.OwnerServer.OrderService.Business.Abstraction.Accommodation;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
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
        private readonly IAccommodationOrderManager manager;

        public OrderController(IAccommodationOrderManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccommodationOrder>>> GetOrders()
        {
            var orders = await manager.GetOrders();
            return orders;
        }

        [HttpPost]
        public Task CreateOrder(int customerId, AccommodationBlueprint accommodationBlueprint)
        {
            var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
            return manager.CreateOrderAsync(customerId, accommodationBlueprint, culture);
        }

        [HttpDelete]
        public Task CancelOrder(int orderId,string reason, bool isCanceledByCustomer)
        {         
            return manager.CancelOrderAsync(orderId, reason, isCanceledByCustomer);            
        }


        [HttpPut]
        public Task UpdateOrder(int orderId, AccommodationBlueprint accommodationBlueprint)
        {
            return manager.EditOrderAsync(orderId, accommodationBlueprint);            
        }

    }
}
