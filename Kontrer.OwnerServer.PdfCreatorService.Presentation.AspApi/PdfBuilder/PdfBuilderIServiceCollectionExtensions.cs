using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.RazorLight;
using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.SelectPdf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder
{
    public static class PdfBuilderIServiceCollectionExtensions
    {
        public static IServiceCollection AddPdfBuilder(this IServiceCollection services)
        {
            services.AddSingleton<IAccommodationPdfCreator, RazorAccommodationPdfCreator>();
            services.AddSingleton<IHtmlToPdfConverter, SelectPdfHtmlToPdfConverter>();
            services.AddSingleton<IAccommodationToHtmlConverter, RazorLightAccommodationToHtmlConverter>();
            services.Configure<AccommodationToHtmlConverterOptions>(x => 
            {
                x.TemplateName = "AccommodationRazorTemplate";
                x.TemplatesDirectory = $"{Directory.GetCurrentDirectory()}/Resources/PdfRazorTemplates";
            });


            return services;
        }
    }
}
