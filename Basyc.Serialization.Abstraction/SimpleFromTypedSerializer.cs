using Basyc.Serializaton.Abstraction;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Serialization.Abstraction
{
    public sealed class SimpleFromTypedSerializer : ISimpleByteSerailizer
    {
        private readonly ITypedByteSerializer typedByteSerializer;

        public SimpleFromTypedSerializer(ITypedByteSerializer typedByteSerializer)
        {
            this.typedByteSerializer = typedByteSerializer;
        }

        public OneOf<object, SerializationFailure> Deserialize(byte[] data, string dataType)
        {
            return typedByteSerializer.Deserialize(data, TypedToSimpleConverter.ConvertSimpleToType(dataType));
        }

        public OneOf<byte[], SerializationFailure> Serialize(object data, string dataType)
        {
            return typedByteSerializer.Serialize(data, TypedToSimpleConverter.ConvertSimpleToType(dataType));
        }
    }
}
