﻿using System;

namespace Kontrer.Shared.MessageBus.Proxy.Shared
{
    public interface IRequestSerializer
    {
        TRequest Deserialize<TRequest>(string json);

        TRequest Deserialize<TRequest>(byte[] request);

        object Deserialize(string json, Type requestType);

        string Serialize<TRequest>(TRequest request);

        string Serialize(object request, Type requestType);

        string Serialize(byte[] request, Type requestType);
    }
}