using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum ERestrictionType { WARNING, INFO, ERROR }

[Serializable]
public class RestrictionData
{
    public enum ERestrictionSort { Exclusion, Inclusion, Type_Unavailable }

    [StringFieldDrawer("Message")]
    public string message;

    [JsonConverter(typeof(StringFloatConverter))]
    [FloatFieldDrawer("Value")]
    public float value;

    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Type")]
    public ERestrictionType type;

    [ReferenceDropdownDrawer("Start layer", "meta.layer_name")]
    public string startlayer;
    [StringFieldDrawer("Start type")] //Make sure we can get the types associated to startlayer
    public string starttype;
    [ReferenceDropdownDrawer("End layer", "meta.layer_name")]
    public string endlayer;
    [StringFieldDrawer("End type")] //Make sure we can get the types associated to startlayer
    public string endtype;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Sort")]
    public ERestrictionSort sort;
}