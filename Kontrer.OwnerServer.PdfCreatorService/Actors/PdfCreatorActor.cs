using Dapr.Actors;
using Dapr.Actors.Runtime;
using Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.Shared.Actors.PdfCreator;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Actors
{
    public class PdfCreatorActor : Actor, IPdfCreatorActor
    {
        private readonly IAccommodationPdfCreator pdfCreator;
        private const string TimedPricesSnapshotState = "TimedPricesSnapshotState";
        public PdfCreatorActor(IAccommodationPdfCreator pdfCreator, ActorService actorService, ActorId actorId, IActorStateManager actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
            this.pdfCreator = pdfCreator;
        }

        public async Task<MemoryStream> CreateAccommodationPdfAsync(PdfCreatorActorRequest request)
        {

            //var prices  = this.StateManager.TryGetStateAsync<Dictionary<string,string>>(TimedPricesSnapshotState);
            return await pdfCreator.CreatePdfAsync(request.Model);
        }
    }
}
