using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Abstraction
{
    public interface IAccommodationPdfCreator
    {        
        Task<MemoryStream> CreatePdfAsync(AccommodationModel accommodation, CultureInfo culture = null);
    }
}
