using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator;
using Kontrer.Shared.MessageBus;
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

        public OrderController(IIdGeneratorManager idGenerator, IMessageBusManager busManager)
        {
            this.idGenerator = idGenerator;
            this.busManager = busManager;
        }

        [HttpGet]
        public int GetNewOrderId()
        {
            AccommodationIdRequestedMessage message = new($"GetNewOrderId request, time:{DateTime.Now.ToString("HH:mm:ss")}");
            busManager.PublishAsync<AccommodationIdRequestedMessage>(message, default).GetAwaiter().GetResult();
            return idGenerator.CreateNewId(IIdGeneratorManager.OrdersGroupName);
        }
    }
}