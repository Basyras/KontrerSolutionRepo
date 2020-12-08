using Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.Abstraction;
using Kontrer.Shared.Models;
using Microsoft.Extensions.Options;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.RazorLight
{
    public class RazorLightAccommodationToHtmlConverter : IAccommodationToHtmlConverter
    {
        private readonly IOptions<AccommodationToHtmlConverterOptions> options;
        private RazorLightEngine engine;

        public RazorLightAccommodationToHtmlConverter(IOptions<AccommodationToHtmlConverterOptions> options)
        {
            engine = new RazorLightEngineBuilder()
                        .UseFileSystemProject(options.Value.TemplatesDirectory)
                        .UseMemoryCachingProvider()
                        .Build();
            this.options = options;
        }
        public async Task<string> ToHtmlAsync(AccommodationModel accommodation)
        {
            var html = await engine.CompileRenderAsync(options.Value.TemplateName, accommodation);
            return html;
        }
    }
}
