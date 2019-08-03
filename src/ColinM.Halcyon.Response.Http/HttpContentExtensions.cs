using System.Net.Http;
using System.Threading.Tasks;
using ColinM.Halcyon.Response.JsonConverters;
using Newtonsoft.Json;

namespace ColinM.Halcyon.Response.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<HalcyonResponseModel<TModel>> ReadAsHalcyonResponse<TModel>(this HttpContent content)
            where TModel : class, new()
        {
            var rawContents = await content.ReadAsStringAsync();

            var halcyonResponse = JsonConvert.DeserializeObject<HalcyonResponseModel<TModel>>(rawContents, new HalcyonJsonConverter<TModel>());

            return halcyonResponse;
        }
    }
}
