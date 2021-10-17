using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class DefaultDomainNameFormatter : IDomainNameFormatter
    {
        public string GetFormattedName(Assembly assembly)
        {
            return assembly.GetName().Name;
        }
    }
}
