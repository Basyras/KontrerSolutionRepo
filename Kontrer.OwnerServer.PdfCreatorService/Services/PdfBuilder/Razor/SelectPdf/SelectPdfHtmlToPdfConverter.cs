using Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.Abstraction;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.SelectPdf
{
    public class SelectPdfHtmlToPdfConverter : IHtmlToPdfConverter
    {
        private HtmlToPdf converter;

        public SelectPdfHtmlToPdfConverter()
        {
            converter = new HtmlToPdf();
        }

        public Task<MemoryStream> ToPdfAsync(string html)
        {
            PdfDocument doc = converter.ConvertHtmlString(html);
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            return Task.FromResult(stream);
        }
    }
}
