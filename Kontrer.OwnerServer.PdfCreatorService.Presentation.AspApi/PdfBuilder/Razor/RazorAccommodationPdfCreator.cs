using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor
{
    public class RazorAccommodationPdfCreator : IAccommodationPdfCreator
    {
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IAccommodationToHtmlConverter accommodationPdfCreator;

        public RazorAccommodationPdfCreator(IHtmlToPdfConverter htmlToPdfConverter, IAccommodationToHtmlConverter modelToHtmlConverter)
        {
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.accommodationPdfCreator = modelToHtmlConverter;
        }

        public async Task<MemoryStream> CreatePdfAsync(AccommodationModel accommodation, CultureInfo culture = null)
        {
            var html = await accommodationPdfCreator.ToHtmlAsync(accommodation);
            var pdf =  await htmlToPdfConverter.ToPdfAsync(html);
            return pdf;
        }
    }
}
