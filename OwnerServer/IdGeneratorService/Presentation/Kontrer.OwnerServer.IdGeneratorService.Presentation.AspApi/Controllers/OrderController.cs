using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
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
        private readonly IIdGeneratorManager idGenerator;
        private readonly IMessageBusManager busManager;


        public OrderController(IIdGeneratorManager idGenerator,IMessageBusManager busManager)
        {
            this.idGenerator = idGenerator;
            this.busManager = busManager;            
        }

        [HttpGet]
        public int GetNewOrderId()
        {
            busManager.PublishAsync<AccommodationIdRequestedMessage>(new($"GetNewOrderId request, time:{DateTime.Now.ToString("HH:mm:ss")}"),default,"testTopic1").GetAwaiter().GetResult();
            return idGenerator.CreateNewId(IIdGeneratorManager.OrdersGroupName);
            
        }
    }
}
