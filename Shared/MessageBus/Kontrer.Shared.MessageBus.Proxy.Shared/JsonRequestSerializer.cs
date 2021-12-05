using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Shared
{
    public class JsonRequestSerializer : IRequestSerializer
    {
        TInput IRequestSerializer.Deserialize<TInput>(byte[] input)
        {
            var result = JsonSerializer.Deserialize<TInput>(input);
            return result;
        }

        object IRequestSerializer.Deserialize(string jsonInput, Type inputType)
        {
            var result = JsonSerializer.Deserialize(jsonInput, inputType);
            return result;
        }

        TInput IRequestSerializer.Deserialize<TInput>(string jsonInput)
        {
            var result = JsonSerializer.Deserialize<TInput>(jsonInput);
            return result;
        }

        object IRequestSerializer.Deserialize(byte[] input, Type inputType)
        {
            var result = JsonSerializer.Deserialize(input, inputType);
            return result;
        }

        string IRequestSerializer.Serialize(object input, Type inputType)
        {
            var result = JsonSerializer.Serialize(input, inputType);
            return result;
        }

        string IRequestSerializer.Serialize(byte[] input, Type inputType)
        {
            var result = JsonSerializer.Serialize(input, inputType);
            return result;
        }

        string IRequestSerializer.Serialize<TInput>(TInput input)
        {
            var result = JsonSerializer.Serialize(input);
            return result;
        }
    }
}