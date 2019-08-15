using System.Net.Http;
using System.Threading.Tasks;
using ColinM.Halcyon.Response.JsonConverters;
using Newtonsoft.Json;

namespace ColinM.Halcyon.Response.Http
{
    /// <summary>
    /// Provides a set of extension methods for deserializing <see cref="HttpContent"/>.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Serialize the HTTP content to a <see cref="HalcyonResponseModel{TModel}"/> as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TModel">Model type which represents the resource.</typeparam>
        /// <param name="content"><see cref="HttpContent"/> containing the response content.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HalcyonResponseModel<TModel>> ReadAsHalcyonResponseAsync<TModel>(this HttpContent content)
            where TModel : class, new()
        {
            var rawContents = await content.ReadAsStringAsync();

            var halcyonResponse = JsonConvert.DeserializeObject<HalcyonResponseModel<TModel>>(rawContents, new HalcyonJsonConverter<TModel>());

            return halcyonResponse;
        }
    }
}
