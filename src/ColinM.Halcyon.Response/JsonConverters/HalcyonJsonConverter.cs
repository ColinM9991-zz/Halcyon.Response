using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColinM.Halcyon.Response.JsonConverters
{
    /// <summary>
    /// Provides a <see cref="JsonConverter{T}"/> which is to be used when deserializing a hal+json API response.
    /// </summary>
    /// <typeparam name="TModel">The model type which represents the hal+json resource.</typeparam>
    public class HalcyonJsonConverter<TModel> : JsonConverter<HalcyonResponseModel<TModel>> where TModel : class, new()
    {
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns></returns>
        public override HalcyonResponseModel<TModel> ReadJson(JsonReader reader, Type objectType, HalcyonResponseModel<TModel> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var rawJson = GetRawJson(reader);

            var halcyonResponseModel = JsonConvert.DeserializeObject<HalcyonResponseModel<TModel>>(rawJson);
            var deserializedModel = JsonConvert.DeserializeObject<TModel>(rawJson);

            halcyonResponseModel.Model = deserializedModel;
            return halcyonResponseModel;
        }

        /// <summary>
        /// Writes the deserialized <see cref="HalcyonResponseModel{TModel}"/> back to its original hal+json content.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The <see cref="HalcyonResponseModel{TModel}"/> which will be serialized back to its original hal+json content.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, HalcyonResponseModel<TModel> value, JsonSerializer serializer)
        {
            var halcyonJsonObject = JObject.FromObject(value, serializer);
            var halcyonModelObject = JObject.FromObject(value.Model, serializer);

            foreach (var modelProperties in halcyonModelObject)
            {
                halcyonJsonObject.Add(modelProperties.Key, modelProperties.Value);
            }

            serializer.Serialize(writer, halcyonJsonObject);
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
