using System;
using System.IO;
using Basyc.Shared.Helpers;
using ProtoBuf;

namespace Basyc.MessageBus.Client.NetMQ;

public static class ProtoBufMessageSerializer
{
    static ProtoBufMessageSerializer()
    {
        Serializer.PrepareSerializer<ProtoBufCommandWrapper>();
        //Serializer.PrepareSerializer<CreateCustomerCommandResponse>()
        //Serializer.PrepareSerializer<CloseCommand>();
        //Serializer.PrepareSerializer<CommandWrapper>();
        //Serializer.PrepareSerializer<ConsistencyCheckRequest>();
        //Serializer.PrepareSerializer<ConsistencyCheckResult>();
    }
    public static byte[] Serialize<T>(T instance)
    {
        if (instance == null)
            return new byte[0];

        using var stream = new MemoryStream();

        Serializer.Serialize(stream, instance);

        return stream.ToArray();
    }

    public static T Deserialize<T>(byte[] bytes)
    {
        //if (bytes == null)
        //    return default!;

        if (bytes.Length == 0)
            return (T)Activator.CreateInstance(typeof(T))!;

        using MemoryStream stream = new MemoryStream();

        // Ensure that our stream is at the beginning.
        stream.Write(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);

        return Serializer.Deserialize<T>(stream);
    }

    public static object Deserialize(byte[] bytes, Type commandType)
    {
        if (bytes == null)
            return commandType.GetDefaultValue();

        if (bytes.Length == 0)
            return Activator.CreateInstance(commandType)!;

        using var stream = new MemoryStream();

        // Ensure that our stream is at the beginning.
        stream.Write(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        //var instance = Activator.CreateInstance(commandType);
        var result = Serializer.Deserialize(commandType, stream);
        return result;
    }
}
