using Basyc.Serializaton.Abstraction;

namespace Basyc.Serialization.Abstraction
{
	public sealed class TypedFromSimpleSerializer : ITypedByteSerializer
	{
		private readonly IObjectToByteSerailizer byteSerailizer;

		public TypedFromSimpleSerializer(IObjectToByteSerailizer byteSerailizer)
		{
			this.byteSerailizer = byteSerailizer;
		}

		public object? Deserialize(byte[] input, Type dataType)
		{
			return byteSerailizer.Deserialize(input, TypedToSimpleConverter.ConvertTypeToSimple(dataType));
		}

		public byte[] Serialize(object? input, Type dataType)
		{
			return byteSerailizer.Serialize(input, TypedToSimpleConverter.ConvertTypeToSimple(dataType));
		}

		//public OneOf<object, SerializationFailure> Deserialize(byte[] objectData, Type objectType)
		//{
		//    var result = byteSerailizer.Deserialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple(objectType));
		//    return result;
		//}

		//public OneOf<T, SerializationFailure> DeserializeT<T>(byte[] objectData)
		//{
		//    var result = byteSerailizer.Deserialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple<T>());
		//    if (result.Value is SerializationFailure fail)
		//    {
		//        return fail;
		//    }
		//    else
		//    {
		//        return (T)result.Value;
		//    }
		//}

		//public OneOf<byte[], SerializationFailure> Serialize(object objectData, Type objectType)
		//{
		//    var result = byteSerailizer.Serialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple(objectType));
		//    return result;
		//}

		//public OneOf<byte[], SerializationFailure> SerializeT<T>(T objectData) where T : notnull
		//{
		//    var result = byteSerailizer.Serialize(objectData, TypedToSimpleConverter.ConvertTypeToSimple<T>());
		//    return result;
		//}
	}
}
