using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using ColinM.Halcyon.Response.JsonConverters;
using ColinM.Halcyon.Response.UnitTests.Models;
using ColinM.Halcyon.Response.UnitTests.Comparers;

namespace ColinM.Halcyon.Response.UnitTests
{
    public class HalcyonJsonConverterTests : TestsBase
    {
        [Fact]
        public void UsingReadJson_WithHalcyonResponseModel_DeserializesModelCorrectly()
        {
            // Arrange
            const string expectedJson = "{\"_links\": {\"self\": {\"href\": \"http://example.org/api/user/fakeid\"}},\"id\": \"fakeid\",\"name\": \"Fake-Name\",\"_embedded\": {\"contacts\": [{\"id\": \"fakeid2\",\"name\": \"Fake-Name-2\"}]}}";

            var expectedResponseModel = base.CreateContactResourceModel();
            var jsonReader = base.CreateJsonReader(expectedJson);
            var halcyonResponseModelType = typeof(HalcyonResponseModel<ContactResource>);
            var existingResponseModel = (HalcyonResponseModel<ContactResource>)null;
            var hasExistingValue = false;
            var jsonSerializer = base.JsonSerializer;

            var halcyonJsonConverter = new HalcyonJsonConverter<ContactResource>();

            // Act
            var halcyonResponseModel = halcyonJsonConverter.ReadJson(jsonReader, halcyonResponseModelType, existingResponseModel, hasExistingValue, jsonSerializer);

            // Assert
            Assert.Equal(expectedResponseModel, halcyonResponseModel, new HalcyonResponseContactResourceComparer());
        }

        [Fact]
        public void UsingWriteJson_WithJsonReaderContainingHalcyonResponse_SerializesModelToOriginalState()
        {
            // Arrange
            const string expectedJson = "{\"_links\": {\"self\": {\"href\": \"http://example.org/api/user/fakeid\"}},\"id\": \"fakeid\",\"name\": \"Fake-Name\",\"_embedded\": {\"contacts\": [{\"_links\": {\"self\": {\"href\": \"http://example.org/api/user/fakeid2\"}},\"id\": \"fakeid2\",\"name\": \"Fake-Name-2\"},{\"_links\": {\"self\": {\"href\": \"http://example.org/api/user/fakeid3\"}},\"id\": \"fakeid3\",\"name\": \"Fake-Name-3\"}],\"website\": {\"_links\": {\"self\": {\"href\": \"http://example.org/api/locations/fakesite\"}},\"id\": \"mwop\",\"url\": \"http://www.fakesite.net\"}}}";
            var expectedJsonObject = JToken.Parse(expectedJson);

            var stringBuilder = new StringBuilder();
            var jsonWriter = base.CreateJsonWriter(stringBuilder);
            var jsonSerializer = base.JsonSerializer;

            var halcyonJsonConverter = new HalcyonJsonConverter<ContactResource>();
            var halcyonResponseModel = JsonConvert.DeserializeObject<HalcyonResponseModel<ContactResource>>(expectedJson, halcyonJsonConverter);

            // Act
            halcyonJsonConverter.WriteJson(jsonWriter, halcyonResponseModel, jsonSerializer);
            var actualJson = stringBuilder.ToString();
            var actualJsonObject = JToken.Parse(actualJson);

            // Assert
            Assert.True(JToken.DeepEquals(expectedJsonObject, actualJsonObject));
        }
    }
}
