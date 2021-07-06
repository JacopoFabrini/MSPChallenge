using Newtonsoft.Json;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class CELData
{
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Grey centerpoint colour")]
    public Color grey_centerpoint_color;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Grey Centerpoint Sprite")]
    public EPointVisuals grey_centerpoint_sprite;

    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Green centerpoint colour")]
    public Color green_centerpoint_color;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Green Centerpoint Sprite")]
    public EPointVisuals green_centerpoint_sprite;
}