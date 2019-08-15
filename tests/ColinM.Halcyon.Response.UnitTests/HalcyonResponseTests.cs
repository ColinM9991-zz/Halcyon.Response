using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using AutoFixture;
using Xunit;
using ColinM.Halcyon.Response.UnitTests.Models;
using ColinM.Halcyon.Response.UnitTests.Comparers;

namespace ColinM.Halcyon.Response.UnitTests
{
    public class HalcyonResponseModelTests : TestsBase
    {
        [Fact]
        public void UsingContainsKey_WithKeyId_ReturnsTrueWhenLinkExists()
        {
            // Arrange
            var linkKey = fixture.Create<string>();
            var linksValue = fixture.Create<string>();
            var links = new Dictionary<string, JToken> { [linkKey] = linksValue };

            var halcyonResponse = new HalcyonResponseModel<MockResponseModel>
            {
                Links = links
            };

            // Act
            var linkContainsKey = halcyonResponse.ContainsLink(linkKey);

            // Assert
            Assert.True(linkContainsKey);
        }

        [Fact]
        public void UsingContainsKey_WithKeyId_ReturnsFalseWhenLinkDoesntExist()
        {
            // Arrange
            var linkKey = fixture.Create<string>();
            var linksValue = fixture.Create<string>();
            var fakeLinkKey = fixture.Create<string>();
            var links = new Dictionary<string, JToken> { [linkKey] = linksValue };

            var halcyonResponse = new HalcyonResponseModel<MockResponseModel>
            {
                Links = links
            };

            // Act
            var linkContainsKey = halcyonResponse.ContainsLink(fakeLinkKey);

            // Assert
            Assert.False(linkContainsKey);
        }

        [Theory]
        [InlineData(true, "when Links contains requested link key and link value", "Real Key", "Real Value", "Real Key", "Real Value")]
        [InlineData(false, "when Links contains link key, but doesn't contain link value", "Real Key", "Real Value", "Real Key", "Fake Value")]
        [InlineData(false, "when Links doesn't contain link key, but doesn't contain link value", "Real Key", "Real Value", "Fake Key", "Fake Value")]
        public void UsingContainsKey_WithKeyIdAndLinkValue_ReturnsExpectedAnswerWhenDescribedConditionIsMet(
            bool expectedAnswer,
            [SuppressMessage("xUnit1026: Theory methods should use all of their parameters", "xUnit1026")]
            string condition,
            string expectedLinkKey,
            string expectedLinkValue,
            string actualLinkKey,
            string actualLinkValue)
        {
            // Arrange
            var links = new Dictionary<string, JToken> { [actualLinkKey] = JToken.Parse(string.Format("{{\"href\": \"{0}\"}}", actualLinkValue)) };

            var halcyonResponse = new HalcyonResponseModel<MockResponseModel>
            {
                Links = links
            };

            // Act
            var actualAnswer = halcyonResponse.ContainsLink(expectedLinkKey, expectedLinkValue);

            // Assert
            Assert.Equal(expectedAnswer, actualAnswer);
        }

        [Theory]
        [InlineData(true, "when Links contains requested link key and link value", "Real Key", "Real Value", "Real Key", "Real Value")]
        [InlineData(false, "when Links contains link key, but doesn't contain link value", "Real Key", "Real Value", "Real Key", "Fake Value")]
        [InlineData(false, "when Links doesn't contain link key, but doesn't contain link value", "Real Key", "Real Value", "Fake Key", "Fake Value")]
        public void UsingContainsKey_WithMultipleKeyIdAndLinkValue_ReturnsExpectedAnswerWhenDescribedConditionIsMet(
            bool expectedAnswer,
            [SuppressMessage("xUnit1026: Theory methods should use all of their parameters", "xUnit1026")]
            string condition,
            string expectedLinkKey,
            string expectedLinkValue,
            string actualLinkKey,
            string actualLinkValue)
        {
            // Arrange
            var links = new Dictionary<string, JToken> { [actualLinkKey] = JToken.Parse(string.Format("[{{\"href\": \"{0}\"}},{{\"href\": \"{0}\"}}]", actualLinkValue)) };

            var halcyonResponse = new HalcyonResponseModel<MockResponseModel>
            {
                Links = links
            };

            // Act
            var actualAnswer = halcyonResponse.ContainsLink(expectedLinkKey, expectedLinkValue);

            // Assert
            Assert.Equal(expectedAnswer, actualAnswer);
        }

        [Fact]
        public void UsingContainsEmbeddedResource_WithModelContainingAnEmbeddedResources_ReturnsTrue()
        {
            // Arrange
            const string embeddedResourceKey = "mock";
            var halcyonResponseModel = new HalcyonResponseModel<MockResponseModel>
            {
                Embedded = new Dictionary<string, JToken> { [embeddedResourceKey] = JToken.FromObject(new { mockTitle = "mock value" }) }
            };

            // Act
            var containsEmbeddedResource = halcyonResponseModel.ContainsEmbeddedResource(embeddedResourceKey);

            // Assert
            Assert.True(containsEmbeddedResource);
        }

        [Fact]
        public void UsingContainsEmbeddedResource_WithModelContainingNoEmbeddedResources_ReturnsFalse()
        {
            // Arrange
            const string embeddedResourceKey = "mock";
            var halcyonResponseModel = new HalcyonResponseModel<MockResponseModel>();

            // Act
            var containsEmbeddedResource = halcyonResponseModel.ContainsEmbeddedResource(embeddedResourceKey);

            // Assert
            Assert.False(containsEmbeddedResource);
        }

        [Fact]
        public void UsingGetEmbeddedResource_WithModelContainingEmbeddedResources_ReturnsCorrectlyMappedEmbeddedResource()
        {
            // Arrange
            const string embeddedResourceKey = "mock";
            var expectedHalcyonResponseModel = new HalcyonResponseModel<MockResponseModel>
            {
                Model = new MockResponseModel { MockContent = "mock value" }
            };

            var halcyonResponseModel = new HalcyonResponseModel<MockResponseModel>
            {
                Embedded = new Dictionary<string, JToken> { [embeddedResourceKey] = JToken.FromObject(new MockResponseModel  { MockContent = "mock value" }) }
            };

            // Act
            var actualEmbeddedResource = halcyonResponseModel.GetEmbeddedResource<MockResponseModel>(embeddedResourceKey);

            // Asert
            Assert.Equal(expectedHalcyonResponseModel.Model, actualEmbeddedResource.Model, new MockResponseModelComparer());
        }

        [Fact]
        public void UsingGetEmbeddedResources_WithModelContainingEmbeddedResources_ReturnsCorrectlyMappedEmbeddedResource()
        {
            // Arrange
            const string embeddedResourceKey = "mock";
            var expectedHalcyonResponseModels = new HalcyonResponseModel<MockResponseModel>[]
            {
                new HalcyonResponseModel<MockResponseModel> { Model = new MockResponseModel { MockContent = "mock value" } },
                new HalcyonResponseModel<MockResponseModel> { Model = new MockResponseModel { MockContent = "mock value 2" } }
            };

            var halcyonResponseModel = new HalcyonResponseModel<MockResponseModel>
            {
                Embedded = new Dictionary<string, JToken>
                {
                    [embeddedResourceKey] = JToken.FromObject(
                        new MockResponseModel[]
                        {
                            new MockResponseModel
                            {
                                MockContent = "mock value"
                            },
                            new MockResponseModel
                            {
                                MockContent = "mock value 2"
                            }
                        }
                    )
                }
            };

            // Act
            var actualEmbeddedResource = halcyonResponseModel.GetEmbeddedResources<MockResponseModel>(embeddedResourceKey);

            // Asert
            Assert.Equal(expectedHalcyonResponseModels, actualEmbeddedResource, new MockResponseModelArrayComparer());
        }
    }
}
