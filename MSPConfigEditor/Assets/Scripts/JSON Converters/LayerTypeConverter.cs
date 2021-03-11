using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

class LayerTypeConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        if (token.Type == JTokenType.Array)
        {
            List<LayerTypeData> layerTypeList = token.ToObject<List<LayerTypeData>>(serializer);
            Dictionary<int, LayerTypeData> result = new Dictionary<int, LayerTypeData>();
            for (int i = 0; i < layerTypeList.Count; i++)
            {
                result.Add(i, layerTypeList[i]);
            }
            return result;
        }
        else if (token.Type == JTokenType.Object)
        {
            return token.ToObject<Dictionary<int, LayerTypeData>>(serializer);
        }

        throw new JsonSerializationException("Unexpected JSON format encountered in LayerTypeConverter: " + token.ToString());
    }

    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<int, LayerTypeData>) || objectType == typeof(List<LayerTypeData>);
    }
}