using Basyc.Serialization.Abstraction;
using OneOf;
using ProtoBuf;
using ProtoBuf.Meta;
using Basyc.Shared.Helpers;

namespace Basyc.Serialization.ProtobufNet
{
    public class ProtobufByteSerializer : ITypedByteSerializer
    {
        public static ProtobufByteSerializer Singlenton = new ProtobufByteSerializer();

        public OneOf<object, SerializationFailure> Deserialize(byte[] objectData, Type dataType)
        {
            PrepareSerializer(dataType);

            if (objectData == null)
                return dataType.GetDefaultValue();

            if (objectData.Length == 0)
            {
                try
                {
                    return Activator.CreateInstance(dataType)!;
                }
                catch
                {
                    throw new Exception("Cannot deserialize message. Message data is empty and message does not have empty constructor.");
                }
            }

            using var stream = new MemoryStream();
            stream.Write(objectData, 0, objectData.Length);
            stream.Seek(0, SeekOrigin.Begin);

            object result = Serializer.Deserialize(dataType, stream);
            return result;
        }

        public OneOf<T, SerializationFailure> DeserializeT<T>(byte[] objectData)
        {
            var result = Deserialize(objectData, typeof(T));
            if (result.Value is SerializationFailure)
                return result.AsT1;

            return (T)result.Value;
        }

        public OneOf<byte[], SerializationFailure> Serialize(object objectData, Type objectType)
        {
            if (objectData == null)
                return new byte[0];

            if (objectData.GetType().GetProperties().Length == 0)
                return new byte[0];

            using var stream = new MemoryStream();
            PrepareSerializer(objectType);

            Serializer.NonGeneric.Serialize(stream, objectType);
            Serializer.Serialize(stream, objectData);
            return stream.ToArray();
        }

        public OneOf<byte[], SerializationFailure> SerializeT<T>(T objectData) where T : notnull
        {
            return Serialize(objectData, typeof(T));
        }

        private static void PrepareSerializer(Type type)
        {
            if (RuntimeTypeModel.Default.CanSerialize(type) is false)
            {
                RuntimeTypeModel.Default.Add(type);
                var parameters = type.GetConstructors().First().GetParameters();
                foreach (var parameter in parameters)
                {
                    PrepareSerializer(parameter.ParameterType);
                }
            }
        }
    }
}