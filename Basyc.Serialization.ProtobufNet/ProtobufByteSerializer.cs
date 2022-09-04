using Basyc.Serialization.Abstraction;
using Basyc.Shared.Helpers;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Basyc.Serialization.ProtobufNet
{
	public class ProtobufByteSerializer : ITypedByteSerializer
	{
		public static ProtobufByteSerializer Singlenton = new ProtobufByteSerializer();
		private readonly static Dictionary<Type, PreparedTypeMetadata> knownTypes = new Dictionary<Type, PreparedTypeMetadata>();
		private static PreparedTypeMetadata PrepareSerializer(Type typeToPrepare)
		{
			if (knownTypes.TryGetValue(typeToPrepare, out var metadata))
				return metadata;


			bool canSeri = false;
			try
			{
				canSeri = RuntimeTypeModel.Default.CanSerialize(typeToPrepare);
			}
			catch (InvalidOperationException)
			{
				//workaround when record type with nested type fails when checking it can serialize ...
			}

			if (canSeri is false)
			{
				foreach (var property in typeToPrepare.GetProperties())
				{
					PrepareSerializer(property.PropertyType);
				}

				if (RuntimeTypeModel.Default.CanSerialize(typeToPrepare) is false)
				{
					var serializationMetadata = RuntimeTypeModel.Default.Add(typeToPrepare);
					serializationMetadata.UseConstructor = false;
					foreach (var property in typeToPrepare.GetProperties())
					{
						serializationMetadata.Add(property.Name);
					}
				}
			}
			var hasZeroProperties = typeToPrepare.GetProperties().Length == 0;
			var newMetadata = new PreparedTypeMetadata(hasZeroProperties);
			knownTypes.Add(typeToPrepare, newMetadata);
			return newMetadata;
		}

		public byte[] Serialize(object? input, Type dataType)
		{
			var typeMetadata = PrepareSerializer(dataType);

			if (input == null)
				return new byte[0];

			if (typeMetadata.HasZeroProperties)
				return new byte[0];

			using var stream = new MemoryStream();
			Serializer.Serialize(stream, input);
			return stream.ToArray();
		}

		public object? Deserialize(byte[] input, Type dataType)
		{
			PrepareSerializer(dataType);

			if (input == null)
				return dataType.GetDefaultValue();

			var stream = new MemoryStream(input);
			stream.Write(input, 0, input.Length);
			stream.Seek(0, SeekOrigin.Begin);

			object result = Serializer.Deserialize(dataType, stream);
			return result;
		}

		public bool TrySerialize<T>(T input, out byte[]? output, out SerializationFailure? error)
		{
			var thisCasted = (ITypedByteSerializer)this;
			return thisCasted.TrySerialize(input, typeof(T), out output, out error);
		}

		public bool TryDeserialize<T>(byte[] serializedInput, out T? input, out SerializationFailure? error)
		{
			var thisCasted = (ITypedByteSerializer)this;
			var wasSuccesful = thisCasted.TryDeserialize(serializedInput, typeof(T), out var inputObject, out error);
			input = (T?)inputObject;
			return wasSuccesful;

		}
	}
}