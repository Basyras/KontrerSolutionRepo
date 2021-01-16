using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IdGeneratorManager idGenerator;

        public OrderController(IdGeneratorManager idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        [HttpGet]
        public int GetNewOrderId()
        {
            return idGenerator.CreateNewId(IdGeneratorManager.OrdersGroupName);
        }
    }
}
