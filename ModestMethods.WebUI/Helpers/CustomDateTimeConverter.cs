using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ModestMethods.WebUI.Helpers
{
    // custopm datetime helper that converts the json datetime data to formatted datetime data
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}