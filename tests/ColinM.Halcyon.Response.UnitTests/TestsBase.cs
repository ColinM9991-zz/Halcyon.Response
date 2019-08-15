using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoFixture;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ColinM.Halcyon.Response.UnitTests.Models;

namespace ColinM.Halcyon.Response.UnitTests
{
    public abstract class TestsBase
    {
        private static readonly JsonSerializer jsonSerializer = new JsonSerializer()
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        protected readonly Fixture fixture;

        public TestsBase()
        {
            this.fixture = new Fixture();
        }

        public JsonSerializer JsonSerializer => jsonSerializer;

        public HalcyonResponseModel<ContactResource> CreateContactResourceModel() =>
            new HalcyonResponseModel<ContactResource>
            {
                Model = new ContactResource
                {
                    Id = "fakeid",
                    Name = "Fake-Name"
                },
                Links = new Dictionary<string, JToken>
                {
                    ["self"] = JToken.FromObject(new { href = "http://example.org/api/user/fakeid" })
                },
                Embedded = new Dictionary<string, JToken>
                {
                    ["contacts"] = JArray.FromObject(new[] {
                        new {
                            id = "fakeid2",
                            name = "Fake-Name-2"
                        }
                    })
                }
            };

        public JsonReader CreateJsonReader(string json)
        {
            var stringReader = new StringReader(json);
            return new JsonTextReader(stringReader);
        }

        public JsonWriter CreateJsonWriter(StringBuilder builder)
        {
            var stringWriter = new StringWriter(builder);
            return new JsonTextWriter(stringWriter);
        }
    }
}
