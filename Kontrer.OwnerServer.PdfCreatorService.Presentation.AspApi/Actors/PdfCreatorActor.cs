using Dapr.Actors.Runtime;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi.Actors
{
    public class PdfCreatorActor : Actor, IPdfCreatorActor
    {
        private readonly IAccommodationOrderPdfCreator pdfCreator;
        private const string TimedPricesSnapshot = "TimedPricesSnapshot";


        public PdfCreatorActor(ActorHost actorHost, IAccommodationOrderPdfCreator pdfCreator) : base(actorHost)
        {

            this.pdfCreator = pdfCreator;
        }

        public async Task<MemoryStream> CreateAccommodationPdfAsync(PdfCreatorActorRequest request)
        {

#warning dodelat
            throw new NotImplementedException();
            var pricesSnapshot = await this.StateManager.TryGetStateAsync<Dictionary<string, string>>(TimedPricesSnapshot);
            if (pricesSnapshot.HasValue is false)
            {
                //pricesSnapshot = 
            }



            return await pdfCreator.CreatePdfAsync(request.Offer);
        }
    }
}
