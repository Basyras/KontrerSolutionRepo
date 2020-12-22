using Dapr;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator;
using Kontrer.OwnerServer.Shared;
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
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> logger;
        private readonly IAccommodationPdfCreator creator;

        public PdfController(ILogger<PdfController> logger, IAccommodationPdfCreator creator)
        {
            this.logger = logger;
            this.creator = creator;
        }

        [Topic(Constants.MessageBusName, nameof(CreateAccommodationPdf))]
        [HttpPost]
        public async Task<byte[]> CreateAccommodationPdf(PdfCreatorActorRequest request)
        {
            logger.LogDebug($"CreateAccommodationPdf called");
            var pdf = await creator.CreatePdfAsync(request.Model);
            return pdf.ToArray();
        }
    }
}
