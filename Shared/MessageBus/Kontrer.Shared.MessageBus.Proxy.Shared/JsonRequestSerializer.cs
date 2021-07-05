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
        TInput IRequestSerializer.Deserialize<TInput>(byte[] input)
        {
            return JsonSerializer.Deserialize<TInput>(input);
        }

        object IRequestSerializer.Deserialize(string jsonInput, Type inputType)
        {
            return JsonSerializer.Deserialize(jsonInput, inputType);
        }

        TInput IRequestSerializer.Deserialize<TInput>(string jsonInput)
        {
            return JsonSerializer.Deserialize<TInput>(jsonInput);
        }

        object IRequestSerializer.Deserialize(byte[] input, Type inputType)
        {
            return JsonSerializer.Deserialize(input, inputType);
        }

        string IRequestSerializer.Serialize(object input, Type inputType)
        {
            return JsonSerializer.Serialize(input, inputType);
        }

        string IRequestSerializer.Serialize(byte[] input, Type inputType)
        {
            return JsonSerializer.Serialize(input, inputType);
        }

        string IRequestSerializer.Serialize<TInput>(TInput input)
        {
            return JsonSerializer.Serialize(input);
        }
    }
}