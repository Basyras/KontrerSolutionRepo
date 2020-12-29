using Dapr;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator;
using Kontrer.OwnerServer.Shared;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationOrderController : ControllerBase
    {
        private readonly ILogger<AccommodationOrderController> logger;
        private readonly IAccommodationOrderPdfCreator creator;

        public AccommodationOrderController(ILogger<AccommodationOrderController> logger, IAccommodationOrderPdfCreator creator)
        {
            this.logger = logger;
            this.creator = creator;
        }

        //[Topic(MessageBusConstants.MessageBusName, nameof(CreateAccommodationPdf))]
        [HttpGet]
        public async Task<byte[]> CreatePdfForAccommodationOrder(AccommodationOrder order)
        {            
            logger.LogDebug($"{nameof(CreatePdfForAccommodationOrder)} called, customer second name: {order.Blueprint.Customer.LastName}");
            var pdf = await creator.CreatePdfAsync(order);            
            return pdf.ToArray();
        }
    }
}
