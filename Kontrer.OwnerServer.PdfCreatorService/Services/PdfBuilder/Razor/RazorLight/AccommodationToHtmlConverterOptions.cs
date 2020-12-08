namespace Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder.Razor.RazorLight
{
    public class AccommodationToHtmlConverterOptions
    {
        public AccommodationToHtmlConverterOptions()
        {

        }
        public AccommodationToHtmlConverterOptions(string templatesDirectory, string templateName)
        {
            TemplatesDirectory = templatesDirectory;
            TemplateName = templateName;
        }

        public string TemplatesDirectory { get; set; }
        public string TemplateName { get; set; }
    }
}