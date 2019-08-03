using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace ColinM.Halcyon.Response.JsonConverters
{
    public class HalcyonJsonConverter<TModel> : JsonConverter<HalcyonResponseModel<TModel>> where TModel : class, new()
    {
        public override HalcyonResponseModel<TModel> ReadJson(JsonReader reader, Type objectType, HalcyonResponseModel<TModel> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var rawJson = GetRawJson(reader);

            var halcyonResponseModel = JsonConvert.DeserializeObject<HalcyonResponseModel<TModel>>(rawJson);
            var deserializedModel = JsonConvert.DeserializeObject<TModel>(rawJson);

            halcyonResponseModel.Model = deserializedModel;
            return halcyonResponseModel;
        }

        public override void WriteJson(JsonWriter writer, HalcyonResponseModel<TModel> value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Only deserialization of Halcyon responses is supported.");
        }

        private static string GetRawJson(JsonReader reader)
        {
            using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteToken(reader);

                return sw.ToString();
            }
        }
    }
}
