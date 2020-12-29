using Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.Abstraction;
using Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi.PdfBuilder.Razor.RazorViews;
using Kontrer.Shared.Localizator;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Microsoft.Extensions.Options;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.RazorLight
{
    public class RazorLightAccommodationOrderToHtmlConverter : IAccommodationOrderToHtmlConverter
    {
        private readonly IOptions<RazorLightPdfBuilderOptions> options;
        private readonly ILocalizationManager localizationManager;
        private readonly ILocalizatedSection viewLocalizationSection;
        private readonly ILocalizatedSection itemLocalizationSection;
        private RazorLightEngine engine;

        public RazorLightAccommodationOrderToHtmlConverter(IOptions<RazorLightPdfBuilderOptions> options, ILocalizationManager localizationManager)
        {
            engine = new RazorLightEngineBuilder()
                        .UseFileSystemProject(options.Value.TemplatesDirectory)
                        .UseMemoryCachingProvider()
                        .Build();
            this.options = options;
            this.localizationManager = localizationManager;
            viewLocalizationSection = localizationManager.GetSection(nameof(AccommodationOrder));
            itemLocalizationSection = localizationManager.GetSection(nameof(AccommodationBlueprint.AccommodationItems));
        }

        public async Task<string> ToHtmlAsync(AccommodationOrder accommodation, CultureInfo culture = null)
        {
            var viewLocalizator = await viewLocalizationSection.GetLocalizatorAsync(culture);
            var itemLocalizator = await itemLocalizationSection.GetLocalizatorAsync(culture);
            RazorViewModel<AccommodationOrder> razorVM = new RazorViewModel<AccommodationOrder>(viewLocalizator, itemLocalizator, accommodation, options.Value.RootResourceDirectory, culture, null);
            var html = await engine.CompileRenderAsync(options.Value.TemplateName, razorVM);
            return html;
        }
    }
}
