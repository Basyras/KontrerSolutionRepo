using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Proxy.Shared
{
    public class JsonRequestSerializer : IRequestSerializer
    {
        public TRequest Deserialize<TRequest>(byte[] request)
        {
            return JsonSerializer.Deserialize<TRequest>(request);
        }

        public object Deserialize(string json, Type requestType)
        {
            return JsonSerializer.Deserialize(json, requestType);
        }

        public string Serialize(object request, Type requestType)
        {
            return JsonSerializer.Serialize(request, requestType);
        }

        public string Serialize(byte[] request, Type requestType)
        {
            return JsonSerializer.Serialize(request, requestType);
        }

        TRequest IRequestSerializer.Deserialize<TRequest>(string json)
        {
            return JsonSerializer.Deserialize<TRequest>(json);
        }

        string IRequestSerializer.Serialize<TRequest>(TRequest request)
        {
            return JsonSerializer.Serialize(request);
        }
    }
}