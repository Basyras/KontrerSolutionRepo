using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator
{
    public class PdfCreatorActorRequest
    {
        public PdfCreatorActorRequest(AccommodationModel model)
        {
            Model = model;
        }

        public AccommodationModel Model { get; }
    }
}
