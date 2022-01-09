using Basyc.Serializaton.Abstraction;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Serialization.Abstraction
{
    public sealed class TypedFromSimpleSerializer : ITypedByteSerializer
    {
        private readonly ISimpleByteSerailizer byteSerailizer;

        public TypedFromSimpleSerializer(ISimpleByteSerailizer byteSerailizer)
        {
            this.byteSerailizer = byteSerailizer;
        }
        public OneOf<object, SerializationFailure> Deserialize(byte[] objectData, Type objectType)
        {
            var result = byteSerailizer.Deserialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple(objectType));
            return result;
        }

        public OneOf<T, SerializationFailure> DeserializeT<T>(byte[] objectData)
        {
            var result = byteSerailizer.Deserialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple<T>());
            if (result.Value is SerializationFailure fail)
            {
                return fail;
            }
            else
            {
                return (T)result.Value;
            }
        }

        public OneOf<byte[], SerializationFailure> Serialize(object objectData, Type objectType)
        {
            var result = byteSerailizer.Serialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple(objectType));
            return result;
        }

        public OneOf<byte[], SerializationFailure> SerializeT<T>(T objectData) where T : notnull
        {
            var result = byteSerailizer.Serialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple<T>());
            return result;
        }
    }
}
