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
            throw new NotImplementedException();
        }

        public string Serialize(object request, Type requestType)
        {
            throw new NotImplementedException();
        }

        public string Serialize(byte[] request, Type requestType)
        {
            throw new NotImplementedException();
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