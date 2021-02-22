using Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction
{
    public interface IAccommodationOrderPdfCreator
    {        
        Task<MemoryStream> CreatePdfAsync(AccommodationOfferViewModel offer, CultureInfo culture = null);
    }
}
