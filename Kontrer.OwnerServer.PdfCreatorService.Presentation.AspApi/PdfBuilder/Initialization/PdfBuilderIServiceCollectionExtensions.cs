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

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Initialization
{
    public static class PdfBuilderIServiceCollectionExtensions
    {
        public static IServiceCollection AddPdfBuilder(this IServiceCollection services)
        {
            services.AddSingleton<IAccommodationOrderPdfCreator, RazorAccommodationOrderPdfCreator>();
            services.AddSingleton<IHtmlToPdfConverter, SelectPdfHtmlToPdfConverter>();
            services.AddSingleton<IAccommodationOrderToHtmlConverter, RazorLightAccommodationOrderToHtmlConverter>();
            services.Configure<RazorLightPdfBuilderOptions>(x => 
            {
                x.TemplateName = "AccommodationRazorTemplate";
                x.TemplatesDirectory = $"{Directory.GetCurrentDirectory()}/Resources/PdfBuilder/Views";
                x.RootResourceDirectory = $"{Directory.GetCurrentDirectory()}/Resources/PdfBuilder/ViewResources";
            });


            return services;
        }
    }
}
