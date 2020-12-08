using Dapr;
using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using Kontrer.OwnerServer.Shared;
using Kontrer.OwnerServer.Shared.Actors.PdfCreator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly DaprClient dapr;
        private readonly ILogger<DebugController> logger;

        public DebugController(DaprClient dapr, ILogger<DebugController> logger)
        {
            this.dapr = dapr;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<string> CreateActor()
        {
            logger.LogDebug("Get method called, creating new IPdfCreatorActor actor");
            var proxy = ActorProxy.Create<ITestActor>(ActorId.CreateRandom(), "PdfCreatorActor");
            var proxy2 = ActorProxy.Create<ITestActor>(ActorId.CreateRandom(), "PdfCreatorActor");
            var proxy3 = ActorProxy.Create<ITestActor>(ActorId.CreateRandom(), "PdfCreatorActor");
            logger.LogDebug("new IPdfCreatorActor created, now calling");
            
            var results = await Task.WhenAll(proxy.TestMethod(), proxy2.TestMethod(), proxy3.TestMethod());
            var result = String.Join(Environment.NewLine, results.Select(a => String.Join(", ", a)));
            logger.LogDebug($"IPdfCreatorActor finished, result:{result}");

            return result;


        }

        [HttpPut]
        public async Task Publish()
        {
            var newString = $"Test_string_{Guid.NewGuid()}";
            logger.LogDebug($"Publishing SetTestString with value {newString}");
            await dapr.PublishEventAsync<TestPdfRequest>(Constants.MessageBusName, "SetTestString", new TestPdfRequest(newString));
            logger.LogDebug($"Event SetTestString published with value {newString}");
        }

        [HttpPost]
        [Topic(Constants.MessageBusName, nameof(TestStringChanged))]
        public void TestStringChanged([FromBody] string value, [FromServices] ILogger<DebugController> logger)
        {
            logger.LogDebug($"TestStringChanged detected2, new value {value}");
        }


    }
}
