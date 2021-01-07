using Kontrer.Shared.Localizator.EF;
using Kontrer.Shared.Localizator.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Localizator.Initialization
{
    public static class EfLocalizatorBuilderExtensions
    {
        public static LocalizatorBuilder AddEfStorage(this LocalizatorBuilder builder)
        {
            builder.AddStorage<EFLocalizatorStorage>();
            return builder;
        }
    }
}
