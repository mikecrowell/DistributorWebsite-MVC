using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Json
{
    #region "CLASS: ByteConverter"
    /// <summary>
    /// Serialize a byte 
    /// </summary>
    public class ByteConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override bool CanWrite { get { return false; } } // Use the default implementation for serialization, which is not broken.

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string)JToken.Load(reader);
            if (value == null)
                return null;
            if (value.Length == 0)
                return new byte[0];
            return Convert.FromBase64String(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region "CLASS: BoolConverter"
    /// <summary>
    /// Serialize boolean as 1 for true or 0 for false
    /// </summary>
    internal class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? "1" : "0");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString() == "1" || reader.Value.ToString().ToUpper() == "TRUE";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
    #endregion
}