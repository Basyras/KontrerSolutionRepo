using Dapr;
using Dapr.Client;
using Kontrer.OwnerServer.Shared;
using Kontrer.OwnerServer.Shared.Actors.PdfCreator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Kontrer.OwnerServer.PdfCreatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> logger;

        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }

        [Topic(Constants.MessageBusName, nameof(SetTestString))]
        [HttpPost]
        public async Task SetTestString(TestPdfRequest request, [FromServices] DaprClient daprClient)
        {
            
            var newString = request.TestValue;
            logger.LogDebug($"Saving Test string with value {newString}");
            await Task.Delay(5000);
            await daprClient.SaveStateAsync(Constants.StateStoreName,"TestString",newString);
            logger.LogDebug($"Saved Test string with value {newString}");
            logger.LogDebug($"Publishing TestString changed with value {newString}");
            await daprClient.PublishEventAsync(Constants.MessageBusName, "TestStringChanged", newString);


        }
    }
}
