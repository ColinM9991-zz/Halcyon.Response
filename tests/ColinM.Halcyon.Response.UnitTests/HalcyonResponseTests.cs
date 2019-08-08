using AutoFixture;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

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
    }
}
