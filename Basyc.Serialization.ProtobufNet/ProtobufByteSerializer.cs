using Basyc.Serialization.Abstraction;
using Basyc.Shared.Helpers;
using ProtoBuf;
using ProtoBuf.Meta;
using System.Reflection;

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

			//Workaround to support records
			bool couldBeSerializedByDefault = RuntimeTypeModel.Default.CanSerialize(typeToPrepare);
			if (couldBeSerializedByDefault is false)
			{
				if (TryFixWithSkippingEmptyCtor(typeToPrepare) is false)
				{
					throw new Exception($"Could not prepare type '{typeToPrepare.Name}'");
				}
			}
			var hasZeroProperties = typeToPrepare.GetProperties().Length == 0;
			var newMetadata = new PreparedTypeMetadata(hasZeroProperties);
			knownTypes.Add(typeToPrepare, newMetadata);
			return newMetadata;
		}

		/// <summary>
		/// Workaround for scenarios when class has empty ctor. Returns false when fix cant be applied.
		/// </summary>
		/// <param name="typeToPrepare"></param>
		/// <returns></returns>
		private static bool TryFixWithSkippingEmptyCtor(Type typeToPrepare)
		{
			if (IsTypeHavingExtraEmptyCtorProblem(typeToPrepare))
			{
				PrepareButSkipCtor(typeToPrepare);
			}
			else
			{
				//Problem could be even nested
				var properties = typeToPrepare.GetProperties();
				foreach (var property in properties)
				{
					//PrepareSerializer(property.PropertyType);
					TryFixWithSkippingEmptyCtor(property.PropertyType);
				}
			}

			return RuntimeTypeModel.Default.CanSerialize(typeToPrepare);
		}

		private static void PrepareButSkipCtor(Type typeToPrepare)
		{
			var serializationMetadata = RuntimeTypeModel.Default.Add(typeToPrepare);
			serializationMetadata.UseConstructor = false;
			foreach (var property in typeToPrepare.GetProperties())
			{
				serializationMetadata.Add(property.Name);
			}
		}

		private static bool IsTypeHavingExtraEmptyCtorProblem(Type type)
		{
			var ctors = type.GetConstructors();

			if (ctors.Length <= 1)
				return false;

			//Must contain empty ctor to have the problem
			if (ctors.FirstOrDefault(x => x.GetParameters().Length != 0) is null)
				return false;

			//Must contain ctor with all properties
			if (ctors.Any(x => x.GetParameters().Length == type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Length) is false)
				return false;

			return true;
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