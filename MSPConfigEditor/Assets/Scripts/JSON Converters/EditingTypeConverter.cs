using System;
using Newtonsoft.Json;
using UnityEngine;

class EditingTypeConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        ELayerEditingType editingType = (ELayerEditingType)value;
        if (editingType == ELayerEditingType.none)
            writer.WriteValue("");
        else
            writer.WriteValue(editingType.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string value = reader.Value.ToString();
        if (string.IsNullOrEmpty(value))
            return ELayerEditingType.none;
        return Enum.Parse(typeof(ELayerEditingType), value);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ELayerEditingType);
    }
}

