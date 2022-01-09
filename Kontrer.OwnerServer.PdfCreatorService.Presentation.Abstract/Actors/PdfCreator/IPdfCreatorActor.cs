using Dapr.Actors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator
{
    public interface IPdfCreatorActor : IActor
    {
        public Task<MemoryStream> CreateAccommodationPdfAsync(PdfCreatorActorRequest request);
    }
}
