using Basyc.Localizator.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.AspApi.PdfBuilder.Razor.RazorViews
{
    public class RazorViewModel<TModel>
    {

        public ILocalizator ViewLocalizer { get; set; }
        public ILocalizator ItemsLocalizer { get; set; }
        public TModel Data { get; set; }
        public string MainResourcesDirectory { get; set; }
        public CultureInfo Culture { get; set; }
        public Dictionary<string, object> ViewObjects { get; set; }

        public RazorViewModel(ILocalizator viewLocalizer, ILocalizator itemsLocalizer, TModel data, string mainResourcesDirectory, CultureInfo culture, Dictionary<string, object> viewObjects = null)
        {
            ViewLocalizer = viewLocalizer;
            ItemsLocalizer = itemsLocalizer;
            Data = data;
            MainResourcesDirectory = mainResourcesDirectory;
            Culture = culture;
            ViewObjects = viewObjects;
        }

       
        
        
    }


}
