using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.Abstraction
{
    public interface IHtmlToPdfConverter
    {
        Task<MemoryStream> ToPdfAsync(string html);
    }
}
