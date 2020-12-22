using Kontrer.OwnerServer.OrderService.Business.Abstraction;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager manager;

        public OrderController(IOrderManager manager)
        {
            this.manager = manager;
        }
        [HttpGet]
        public async Task<IActionResult>GetOrders()
        {
            var orders = manager.GetOrders();
        }
    }
}
