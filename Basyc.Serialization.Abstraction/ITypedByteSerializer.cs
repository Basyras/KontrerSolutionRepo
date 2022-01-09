using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Serialization.Abstraction
{
    public interface ITypedByteSerializer
    {
        OneOf<byte[], SerializationFailure> Serialize(object objectData, Type objectType);
        OneOf<byte[], SerializationFailure> SerializeT<T>(T objectData) where T : notnull;

        OneOf<object, SerializationFailure> Deserialize(byte[] objectData, Type objectType);
        OneOf<T, SerializationFailure> DeserializeT<T>(byte[] objectData);

    }
}
