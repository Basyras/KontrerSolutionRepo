using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBux.Proxy.Shared
{
    public class JsonRequestSerializer : IRequestSerializer
    {
        TRequest IRequestSerializer.Deserialize<TRequest>(string json)
        {
            return JsonSerializer.Deserialize<TRequest>(json);
        }

        string IRequestSerializer.Serialize<TRequest>(TRequest request)
        {
            return JsonSerializer.Serialize<TRequest>(request);
        }
    }
}