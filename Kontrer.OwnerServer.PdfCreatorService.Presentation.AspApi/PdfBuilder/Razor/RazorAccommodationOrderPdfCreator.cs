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
    public class RazorAccommodationOrderPdfCreator : IAccommodationOrderPdfCreator
    {
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IAccommodationOrderToHtmlConverter accommodationPdfConverter;

        public RazorAccommodationOrderPdfCreator(IHtmlToPdfConverter htmlToPdfConverter, IAccommodationOrderToHtmlConverter modelToHtmlConverter)
        {
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.accommodationPdfConverter = modelToHtmlConverter;
        }

        public async Task<MemoryStream> CreatePdfAsync(AccommodationOrder order, CultureInfo culture = null)
        {
            var html = await accommodationPdfConverter.ToHtmlAsync(order);
            var pdf =  await htmlToPdfConverter.ToPdfAsync(html);
            return pdf;
        }
    }
}
