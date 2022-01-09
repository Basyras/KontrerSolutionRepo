using Basyc.Serializaton.Abstraction;
using Bogus;
using FluentAssertions;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Basyc.Serialization.ProtobufNet.Tests
{
    public class ProtobufByteSerializerTests
    {
        private readonly ProtobufByteSerializer serializer;

        private readonly Faker<TestCar> carFaker;

        private readonly Faker<TestCustomer> customerFaker;

        public ProtobufByteSerializerTests()
        {
            serializer = new ProtobufByteSerializer();
            carFaker = new Faker<TestCar>()
                .RuleFor(x => x.Name, x => x.Name.LastName());

            customerFaker = new Faker<TestCustomer>()
                .RuleFor(x => x.FirstName, x => x.Name.FirstName())
                .RuleFor(x => x.LastName, x => x.Name.LastName())
                .RuleFor(x => x.Age, x => x.Random.Int(0, 100))
                .RuleFor(x => x.Car, carFaker.Generate(1).First());
        }

        [Fact]
        public void Serialization_Should_Match_Deserialization()
        {
            var originalCustomer = customerFaker.Generate();
            var seriCustomer = serializer.Serialize(originalCustomer, typeof(TestCustomer)).AsT0;
            var deseriCustomer = (TestCustomer)serializer.Deserialize(seriCustomer, typeof(TestCustomer)).AsT0;

            var origialJson = JsonSerializer.Serialize(originalCustomer);
            var deseriJson = JsonSerializer.Serialize(deseriCustomer);
            origialJson.Should().Be(deseriJson);
        }


        [Fact]
        public void Should_Serialize_Nested()
        {
            var originalCustomer = customerFaker.Generate();
            var seriCustomer = serializer.Serialize(originalCustomer, typeof(TestCustomer)).AsT0;
            var customerMessageType = TypedToSimpleConverter.ConvertTypeToSimple(originalCustomer.GetType());
            var originalWrapper = new ParentWrapperMessage(0, customerMessageType, seriCustomer);
            
            var seriWrapper = serializer.Serialize(originalWrapper, typeof(ParentWrapperMessage)).AsT0;
            var deseriWrapper = (ParentWrapperMessage)serializer.Deserialize(seriWrapper, typeof(ParentWrapperMessage)).AsT0;

            var origialJson = JsonSerializer.Serialize(originalWrapper);
            var deseriJson = JsonSerializer.Serialize(deseriWrapper);
            deseriJson.Should().Be(origialJson);
        }


    }
}