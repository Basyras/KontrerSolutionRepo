using Kontrer.OwnerServer.OrderService.Dtos.Models;
using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
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
        [HttpGet]
        public async Task<ActionResult<List<AccommodationOrder>>> GetNewOrders()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task CreateOrder(int customerId, AccommodationBlueprint accommodationBlueprint)
        {
            var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task UpdateOrder(int orderId, AccommodationBlueprint accommodationBlueprint)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task CancelOrder(int orderId, string reason, bool isCanceledByCustomer)
        {
            throw new NotImplementedException();
        }
    }
}