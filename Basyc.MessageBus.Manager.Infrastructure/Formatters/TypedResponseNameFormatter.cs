using Basyc.MessageBus.Manager.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure
{
    public class TypedResponseNameFormatter : ITypedResponseNameFormatter
    {
        public string GetFormattedName(Type responseType)
        {
            return responseType.Name;
        }
    }
}