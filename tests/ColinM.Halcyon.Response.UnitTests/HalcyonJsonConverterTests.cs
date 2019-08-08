using ColinM.Halcyon.Response.JsonConverters;
using Newtonsoft.Json;
using AutoFixture;
using Xunit;
using ColinM.Halcyon.Response.UnitTests.Comparers;

namespace ColinM.Halcyon.Response.UnitTests
{
    public class HalcyonJsonConverterTests : TestsBase
    {
        [Fact]
        public void UsingReadJson_WithJsonReaderContainingHalcyonResponse_DeserializesModelCorrectly()
        {
            // Arrange
            var mockResponseModel = base.fixture.Create<MockResponseModel>();
            var halcyonJson = base.GenerateHalcyonResponse(mockResponseModel);

            var jsonReader = base.CreateJsonReader(halcyonJson);
            var halcyonResponseModelType = typeof(HalcyonResponseModel<MockResponseModel>);
            var existingResponseModel = (HalcyonResponseModel<MockResponseModel>)null;
            var hasExistingValue = false;
            var jsonSerializer = new JsonSerializer();

            var halcyonJsonConverter = new HalcyonJsonConverter<MockResponseModel>();

            // Act
            var halcyonResponseModel = halcyonJsonConverter.ReadJson(jsonReader, halcyonResponseModelType, existingResponseModel, hasExistingValue, jsonSerializer);

            // Assert
            Assert.Equal(mockResponseModel, halcyonResponseModel.Model, new MockResponseModelEqualityComparer());
        }
    }
}
