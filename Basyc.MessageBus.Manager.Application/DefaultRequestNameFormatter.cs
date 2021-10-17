using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class DefaultRequestNameFormatter : IRequestNameFormatter
    {
        public string GetFormattedName(Type request)
        {
            var requestName = request.Name
                .Replace("Command", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("Request", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("Message", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("Query", string.Empty, StringComparison.OrdinalIgnoreCase);

            return requestName;
        }
    }
}