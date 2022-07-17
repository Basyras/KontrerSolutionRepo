namespace Basyc.Serialization.Abstraction
{
	public interface ISerializer<TDeserialized, TSerialized, TSerializationMetadata> 
	{
		//OneOf<TOutput, SerializationFailure> TrySerialize(TInput input, TObjectTypeMetadata dataType);
		//OneOf<TInput, SerializationFailure> TryDeserialize(TOutput serializedInput, TObjectTypeMetadata dataType);

		public bool TrySerialize(TDeserialized deserializedObject, TSerializationMetadata dataType, out TSerialized? serializedObject, out SerializationFailure? error)
		{
			try
			{
				serializedObject = Serialize(deserializedObject, dataType);
				error = null;
				return true;
			}
			catch(Exception ex)
			{
				serializedObject = default;
				error = new SerializationFailure(ex);
				return false;
			}
		}
		bool TryDeserialize(TSerialized serializedObject, TSerializationMetadata dataType, out TDeserialized? deserializedObject, out SerializationFailure? error)
		{
			try
			{
				deserializedObject = Deserialize(serializedObject, dataType);
				error = null;
				return true;
			}
			catch (Exception ex)
			{
				deserializedObject = default;
				error = new SerializationFailure(ex);
				return false;
			}
		}

		/// <summary>
		/// Throws <see cref="SerializationFailureException"/> exception when fails
		/// </summary>
		/// <param name="deserializedObject"></param>
		/// <param name="dataType"></param>
		/// <returns></returns>
		TSerialized Serialize(TDeserialized deserializedObject, TSerializationMetadata dataType);
		/// <summary>
		/// Throws <see cref="SerializationFailureException"/> exception when fails
		/// </summary>
		/// <param name="serializedInput"></param>
		/// <param name="dataType"></param>
		/// <returns></returns>
		TDeserialized Deserialize(TSerialized serializedInput, TSerializationMetadata dataType);
	}
}
