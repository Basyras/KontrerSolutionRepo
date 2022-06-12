using OneOf;

namespace Basyc.Serialization.Abstraction
{
    public interface ISimpleToByteSerailizer
    {
        public OneOf<byte[], SerializationFailure> Serialize(object data, string dataType);

        public OneOf<object, SerializationFailure> Deserialize(byte[] data, string dataType);
    }
}