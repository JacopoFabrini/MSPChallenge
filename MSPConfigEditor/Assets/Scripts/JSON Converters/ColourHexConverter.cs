using System;
using Newtonsoft.Json;
using UnityEngine;

public class ColourHexConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {        
        writer.WriteValue("#" + ColorUtility.ToHtmlStringRGBA((Color)value));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string colorString = reader.Value.ToString();//.Substring(1);
        Color result = Color.white;
        if (!ColorUtility.TryParseHtmlString(colorString, out result))
        {
            Debug.LogError("Could not convert hex colour (" + colorString + ") to a colour");
        }
        return result;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Color);
    }
}

