using Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction
{
    public interface IAccommodationOrderToHtmlConverter
    {
        Task<string> ToHtmlAsync(AccommodationOfferViewModel offer, CultureInfo culture = null);
    }
}
