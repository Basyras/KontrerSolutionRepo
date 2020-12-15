using Dapr.Actors;
using Dapr.Actors.Runtime;
using Kontrer.OwnerServer.Shared.Actors.PdfCreator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Actors
{
    public class TestActor : Actor, ITestActor
    {              

        public TestActor(ActorService actorService, ActorId actorId/*,[FromServices] ILogger<PdfCreatorActor> logger*/) : base(actorService, actorId)
        {
            //logger = actorService.LoggerFactory.CreateLogger<PdfCreatorActor>();
            Logger.LogDebug($"{nameof(TestActor)} actor created with id {Id}");            
        }

        public async Task<string> TestMethod()
        {
            Logger.LogDebug($"{nameof(TestMethod)} starting");
            await Task.Delay(0);
            //return Task.FromResult("TestMethod test value");
            return $"TestMethod test value:{Guid.NewGuid()} actorId: {Id}";
        }

        protected override Task OnDeactivateAsync()
        {
            Logger.LogDebug($"{nameof(TestActor)} with id {Id} OnDeactivateAsync");
            return base.OnDeactivateAsync();
        }
    }
}
