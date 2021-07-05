using System;

namespace Kontrer.Shared.MessageBus.Proxy.Shared
{
    public interface IRequestSerializer
    {
        TRequest Deserialize<TRequest>(string jsonInput);

        TInput Deserialize<TInput>(byte[] input);

        object Deserialize(byte[] input, Type inputType);

        object Deserialize(string jsonInput, Type inputType);

        string Serialize<TInput>(TInput input);

        string Serialize(object input, Type inputType);

        string Serialize(byte[] input, Type inputType);
    }
}