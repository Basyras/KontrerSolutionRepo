using System;
using System.IO;
using System.Runtime.Serialization;
using Basyc.Shared.Helpers;
using ProtoBuf;

namespace Basyc.MessageBus.NetMQ.Shared;

public static class TypedObjectToByteSerializer
{
    static TypedObjectToByteSerializer()
    {
        Serializer.PrepareSerializer<ProtoMessageWrapper>();
    }
    public static byte[] Serialize<T>(T instance)
    {
        if (instance == null)
            return new byte[0];

        if (instance.GetType().GetProperties().Length == 0)
            return new byte[0];        

        using var stream = new MemoryStream();
        Serializer.Serialize(stream, instance);


        return stream.ToArray();
    }

    public static T Deserialize<T>(byte[] bytes)
    {
        if (bytes.Length == 0)
            return (T)Activator.CreateInstance(typeof(T))!;

        using MemoryStream stream = new MemoryStream();

        // Ensure that our stream is at the beginning.
        stream.Write(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        try
        {
            return Serializer.Deserialize<T>(stream);
        }
        catch (System.IO.EndOfStreamException ex)
        {
            throw new Exception($"Received message is not probably not correct format ${nameof(ProtoMessageWrapper)}", ex);
        }
    }

    public static object Deserialize(byte[] bytes, Type commandType)
    {
        if (bytes == null)
            return commandType.GetDefaultValue();

        if (bytes.Length == 0)
            return Activator.CreateInstance(commandType)!;

        using var stream = new MemoryStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        var result = Serializer.Deserialize(commandType, stream);
        return result;
    }
}
