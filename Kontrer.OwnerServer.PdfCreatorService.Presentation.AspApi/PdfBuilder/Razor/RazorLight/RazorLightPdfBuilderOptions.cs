namespace Kontrer.OwnerServer.PdfCreatorService.PdfBuilder.Razor.RazorLight
{
    public class RazorLightPdfBuilderOptions
    {
        public RazorLightPdfBuilderOptions()
        {

        }
        public RazorLightPdfBuilderOptions(string templatesDirectory, string templateName)
        {
            TemplatesDirectory = templatesDirectory;
            TemplateName = templateName;
        }

        /// <summary>
        /// Contains resource like images for html views
        /// </summary>
        public string RootResourceDirectory { get; set; }
        public string TemplatesDirectory { get; set; }
        public string TemplateName { get; set; }
    }
}