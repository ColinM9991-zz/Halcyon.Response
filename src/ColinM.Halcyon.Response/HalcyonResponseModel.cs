using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ColinM.Halcyon.Response.Comparers;
using ColinM.Halcyon.Response.JsonConverters;

namespace ColinM.Halcyon.Response
{
    /// <summary>
    /// Serves as a model class for a hal+json formatted HTTP response.
    /// </summary>
    /// <typeparam name="TModel">The type of model which the response will be deserialized to.</typeparam>
    public partial class HalcyonResponseModel<TModel>
    {
        /// <summary>
        /// Contains the resource model.
        /// </summary>
        public TModel Model { get; set; }

        /// <summary>
        /// Contains the links which describe the resource.
        /// </summary>
        [JsonProperty("_links")]
        public IReadOnlyDictionary<string, JToken> Links { get; set; } = new Dictionary<string, JToken>();

        /// <summary>
        /// Contains the embedded resources. 
        /// </summary>
        [JsonProperty("_embedded")]
        public IReadOnlyDictionary<string, JToken> Embedded { get; set; } = new Dictionary<string, JToken>();

        /// <summary>
        /// Determines whether the Links provided in the HAL response contains a link matching the specified key.
        /// </summary>
        /// <param name="linkKey">The key to locate in the collection links.</param>
        /// <returns>True if the links contains a link matching the specified key, otherwise false.</returns>
        public bool ContainsLink(string linkKey)
            => Links.ContainsKey(linkKey);

        /// <summary>
        /// Determines whether the Links provided in the HAL response contains a link matching the specified key and link href value.
        /// </summary>
        /// <param name="linkKey">The key to locate in the collection of links.</param>
        /// <param name="hrefValue">The value which the link matching the specified <paramref name="linkKey"/> must be equal to.</param>
        /// <returns>True if the links contains a link matching the specified key and where the link href value also matches the expected link value, otherwise false.</returns>
        public bool ContainsLink(string linkKey, string hrefValue)
        {
            if (!ContainsLink(linkKey))
            {
                return false;
            }

            var linkToken = Links[linkKey];

            return new LinkComparerFactory()
                .CreateLinkComparer(linkToken)
                .CompareLink(hrefValue, linkToken);
        }

        /// <summary>
        /// Determines whether the embedded resources contains a a resource matching the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key to locate in the collection of embedded resources.</param>
        /// <returns>True if the embedded resources contains a resource matching the specified resource key, otherwise false.</returns>
        public bool ContainsEmbeddedResource(string resourceKey)
            => Embedded.ContainsKey(resourceKey);

        /// <summary>
        /// Locates the embedded resource identified by the resource key and converts it to a <see cref="HalcyonResponseModel{TModel}"/>.
        /// </summary>
        /// <typeparam name="TEmbeddedResource">The model type for the embedded resource.</typeparam>
        /// <param name="resourceKey">The key of the resource to locate in the embedded items.</param>
        /// <returns><see cref="HalcyonResponseModel{TModel}"/> containing the specified model type.</returns>
        public HalcyonResponseModel<TEmbeddedResource> GetEmbeddedResource<TEmbeddedResource>(string resourceKey)
            where TEmbeddedResource : class, new()
        {
            var embeddedResource = GetEmbeddedResource(resourceKey);

            return DeserializeResource<HalcyonResponseModel<TEmbeddedResource>, TEmbeddedResource>(embeddedResource);
        }

        /// <summary>
        /// Locates the embedded resource identified by the resource key and converts it to a collection of <see cref="HalcyonResponseModel{TModel}"/>.
        /// </summary>
        /// <typeparam name="TEmbeddedResource">The model type for the embedded resource.</typeparam>
        /// <param name="resourceKey">The key of the resource to locate in the embedded items.</param>
        /// <returns>Collection of <see cref="HalcyonResponseModel{TModel}"/> containing the specified model type.</returns>
        public HalcyonResponseModel<TEmbeddedResource>[] GetEmbeddedResources<TEmbeddedResource>(string resourceKey)
            where TEmbeddedResource : class, new()
        {
            var embeddedResource = (JArray)GetEmbeddedResource(resourceKey);

            return DeserializeResource<HalcyonResponseModel<TEmbeddedResource>[], TEmbeddedResource>(embeddedResource);
        }

        /// <summary>
        /// Gets the resource which is associated with the specified key.
        /// </summary>
        /// <typeparam name="TEmbeddedResource"></typeparam>
        /// <param name="resourceKey">The key to locate.</param>
        /// <param name="embeddedResource">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>True if the resource was found, otherwise false.</returns>
        public bool TryGetEmbeddedResource<TEmbeddedResource>(string resourceKey, out HalcyonResponseModel<TEmbeddedResource> embeddedResource)
            where TEmbeddedResource : class, new()
        {
            try
            {
                embeddedResource = GetEmbeddedResource<TEmbeddedResource>(resourceKey);
                return true;
            }
            catch (KeyNotFoundException)
            {
                embeddedResource = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the resources which are associated with the specified key.
        /// </summary>
        /// <typeparam name="TEmbeddedResource"></typeparam>
        /// <param name="resourceKey">The key to locate.</param>
        /// <param name="embeddedResource">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>True if the resource was found, otherwise false.</returns>
        public bool TryGetEmbeddedResources<TEmbeddedResource>(string resourceKey, out HalcyonResponseModel<TEmbeddedResource>[] embeddedResources)
            where TEmbeddedResource : class, new()
        {
            try
            {
                embeddedResources = GetEmbeddedResources<TEmbeddedResource>(resourceKey);
                return true;
            }
            catch (KeyNotFoundException)
            {
                embeddedResources = null;
                return false;
            }
        }

        private JToken GetEmbeddedResource(string resourceKey)
        {
            if (!Embedded.ContainsKey(resourceKey))
            {
                throw new KeyNotFoundException($"Embedded resource {{{resourceKey}}} does not exist");
            }

            var embeddedResource = Embedded[resourceKey];

            return embeddedResource;
        }

        private TResponse DeserializeResource<TResponse, TEmbeddedResource>(JToken embeddedResource)
            where TEmbeddedResource : class, new()
            => JsonConvert.DeserializeObject<TResponse>(embeddedResource.ToString(), new HalcyonJsonConverter<TEmbeddedResource>());
    }
}
