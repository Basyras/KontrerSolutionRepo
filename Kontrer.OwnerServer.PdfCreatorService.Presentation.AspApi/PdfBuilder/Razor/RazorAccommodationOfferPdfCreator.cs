using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor
{
    public class RazorAccommodationOfferPdfCreator : IAccommodationOrderPdfCreator
    {
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IAccommodationOrderToHtmlConverter accommodationPdfConverter;

        public RazorAccommodationOfferPdfCreator(IHtmlToPdfConverter htmlToPdfConverter, IAccommodationOrderToHtmlConverter modelToHtmlConverter)
        {
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.accommodationPdfConverter = modelToHtmlConverter;
        }

        public async Task<MemoryStream> CreatePdfAsync(AccommodationOfferViewModel offer, CultureInfo culture = null)
        {
            var html = await accommodationPdfConverter.ToHtmlAsync(offer);
            var pdf =  await htmlToPdfConverter.ToPdfAsync(html);
            return pdf;
        }
    }
}
