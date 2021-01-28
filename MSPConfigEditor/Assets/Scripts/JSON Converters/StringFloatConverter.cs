using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

class StringFloatConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        if (token.Type == JTokenType.String)
        {
            float result = 0;
            float.TryParse(token.ToString(), out result);
            return result;
        }
        else if (token.Type == JTokenType.Float)
        {
            return token.ToObject<float>(serializer);
        }
        else if (token.Type == JTokenType.Integer)
        {
	        return (float)token.ToObject<int>(serializer);
        }

		throw new JsonSerializationException("Unexpected JSON format encountered in StringFloatConverter: " + token.ToString());
    }

    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(float) || objectType == typeof(string);
    }
}