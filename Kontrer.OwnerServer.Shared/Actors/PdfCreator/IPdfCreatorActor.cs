using Dapr.Actors;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Actors.PdfCreator
{
    public interface IPdfCreatorActor : IActor
    {
        public Task<MemoryStream> CreateAccommodationPdfAsync(PdfCreatorActorRequest request);
    }
}
