using AutoFixture;
using Halcyon.HAL;
using Newtonsoft.Json;
using System.IO;

namespace ColinM.Halcyon.Response.UnitTests
{
    public abstract class TestsBase
    {
        protected readonly Fixture fixture;

        public TestsBase()
        {
            this.fixture = new Fixture();
        }

        public string GenerateHalcyonResponse(MockResponseModel mockResponseModel)
        {
            var halResponse = new HALResponse(mockResponseModel, new HALModelConfig { ForceHAL = true });

            return halResponse.ToJObject(new JsonSerializer()).ToString();
        }

        public JsonReader CreateJsonReader(string json)
        {
            var stringReader = new StringReader(json);
            return new JsonTextReader(stringReader);
        }
    }
}
