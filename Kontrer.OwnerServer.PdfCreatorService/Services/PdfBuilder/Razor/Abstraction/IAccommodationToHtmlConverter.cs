using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.Abstraction
{
    public interface IAccommodationToHtmlConverter
    {
        Task<string> ToHtmlAsync(AccommodationModel accommodation);
    }
}
