using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Localizator
{
    public record GetLocalizatorResult(bool localizatorFound, ILocalizator localizator);
    
}
