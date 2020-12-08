using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Actors.PdfCreator
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
