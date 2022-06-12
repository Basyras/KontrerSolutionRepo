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

            var stream = new MemoryStream(objectData);
            //stream.Position = 0;
            stream.Write(objectData, 0, objectData.Length);
            stream.Seek(0, SeekOrigin.Begin);

            object result = Serializer.Deserialize(dataType, stream);
            var opt = new SchemaGenerationOptions();
            opt.Types.Add(dataType);
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
            Serializer.Serialize(stream, objectData);
            return stream.ToArray();
        }

        public OneOf<byte[], SerializationFailure> SerializeT<T>(T objectData) where T : notnull
        {
            return Serialize(objectData, typeof(T));
        }

        //private static void PrepareSerializer(Type type)
        //{
        //    if (RuntimeTypeModel.Default.CanSerialize(type) is false)
        //    {
        //        var serializationMetadata = RuntimeTypeModel.Default.Add(type,false);
        //        serializationMetadata.UseConstructor = false;

        //        foreach(var property in type.GetProperties())
        //        {
        //            //var met = serializationMetadata[property.GetGetMethod()];
        //            //if(met == null)
        //            //{
        //            //    PrepareSerializer(property.PropertyType);
        //            //    serializationMetadata.Add(property.Name);
        //            //}
        //            PrepareSerializer(property.PropertyType);
        //            serializationMetadata.Add(property.Name);
        //        }



        //        //var parameters = type.GetConstructors()
        //        //    .OrderByDescending(x=>x.GetParameters().Length)
        //        //    .First()
        //        //    .GetParameters();

        //        //foreach (var parameter in parameters)
        //        //{
        //        //    PrepareSerializer(parameter.ParameterType);
        //        //}
        //    }

        //}

        /// <summary>
        /// Returns if serailizer was prepared
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //private static bool PrepareSerializer(Type type)
        //{
        //    if (RuntimeTypeModel.Default.CanSerialize(type))
        //    {
        //        return true;
        //    }

        //    foreach (var property in type.GetProperties())
        //    {
        //        var wasPrepared = PrepareSerializer(property.PropertyType);
        //    }

        //    var serializationMetadata = RuntimeTypeModel.Default.Add(type);
        //    //serializationMetadata.UseConstructor = false;



        //    foreach (var property in type.GetProperties())
        //    {
        //        //var wasPrepared = PrepareSerializer(property.PropertyType);
        //        //serializationMetadata.Add(property.Name);
        //        //if (wasPrepared is false)
        //        //{
        //        //    serializationMetadata.Add(property.Name);
        //        //}
        //    }


        //    //if (RuntimeTypeModel.Default.CanSerialize(type) is false)
        //    //{
        //    //    foreach (var property in type.GetProperties())
        //    //    {
        //    //        serializationMetadata.Add(property.Name);
        //    //    }                    
        //    //}


        //    return false;



        //}

        ///

        //private static void PrepareSerializer(Type type)
        //{
        //    if (RuntimeTypeModel.Default.CanSerialize(type) is false)
        //    {
        //        var serializationMetadata = RuntimeTypeModel.Default.Add(type);
        //        //serializationMetadata.UseConstructor = false;

        //        System.Reflection.PropertyInfo[] properties = type.GetProperties();
        //        for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
        //        {
        //            System.Reflection.PropertyInfo? property = properties[propertyIndex];
        //            if (RuntimeTypeModel.Default.CanSerialize(type) is false)
        //            {
        //                PrepareSerializer(property.PropertyType);
        //                serializationMetadata.AddSubType(propertyIndex, property.PropertyType);
        //            }
        //        }
        //    }

        //}

        private static void PrepareSerializer(Type type)
        {
            if (RuntimeTypeModel.Default.CanSerialize(type) is false)
            {
                foreach (var property in type.GetProperties())
                {
                    PrepareSerializer(property.PropertyType);
                }

                if (RuntimeTypeModel.Default.CanSerialize(type) is false)
                {
                    var serializationMetadata = RuntimeTypeModel.Default.Add(type);
                    serializationMetadata.UseConstructor = false;
                    foreach (var property in type.GetProperties())
                    {
                        serializationMetadata.Add(property.Name);
                    }
                }
            }

        }
    }
}