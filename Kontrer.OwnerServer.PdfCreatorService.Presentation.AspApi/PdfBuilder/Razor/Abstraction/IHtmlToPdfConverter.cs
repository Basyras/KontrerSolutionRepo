using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction
{
    public interface IHtmlToPdfConverter
    {
        Task<MemoryStream> ToPdfAsync(string html);
    }
}
