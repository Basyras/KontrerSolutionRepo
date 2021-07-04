using System;

namespace Kontrer.Shared.MessageBux.Proxy.Shared
{
    public interface IRequestSerializer
    {
        TRequest Deserialize<TRequest>(string json);

        string Serialize<TRequest>(TRequest request);
    }
}