using Basyc.Serializaton.Abstraction;
using OneOf;

namespace Basyc.Serialization.Abstraction
{
	public sealed class ObjectFromTypedByteSerializer : IObjectToByteSerailizer
	{
		private readonly ITypedByteSerializer typedByteSerializer;

		public ObjectFromTypedByteSerializer(ITypedByteSerializer typedByteSerializer)
		{
			this.typedByteSerializer = typedByteSerializer;
		}

		public OneOf<object, SerializationFailure> Deserialize(byte[] data, string dataType)
		{
			return typedByteSerializer.Deserialize(data, TypedToSimpleConverter.ConvertSimpleToType(dataType));
		}

		public OneOf<byte[], SerializationFailure> Serialize(object data, string dataType)
		{
			var clrType = TypedToSimpleConverter.ConvertSimpleToType(dataType);
			var seriResult = typedByteSerializer.Serialize(data, clrType);
			return seriResult;
		}
	}
}
