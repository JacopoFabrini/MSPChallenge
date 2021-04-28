using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum EPlanRestrictionType { Warning, Error, Info }

[Serializable]
public class PlanData
{
    [IntFieldDrawer("Plan ID")]
    //[JsonConverter(typeof(StringFloatConverter)), IntFieldDrawer("Plan ID")]
    public int plan_id;
    [IntFieldDrawer("Country ID")]
    //[JsonConverter(typeof(StringFloatConverter)), IntFieldDrawer("Country ID")]
    public int plan_country_id;
    [StringFieldDrawer("Plan Name")]
    public string plan_name;
    [IntFieldDrawer("Game Time")]
    public int plan_gametime;
    [StringFieldDrawer("Plan Types")]
    public string plan_type; //3 comma separated ints
    [StringFieldDrawer("Does the plan alter energy distributions or not")]
    public int plan_alters_energy_distribution = 0;

    [ListDrawer("Layers", Priority = 0), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<PlanLayerData> layers;
    [ListDrawer("Grids", Priority = 0), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<PlanGridData> grids;
    [ListDrawer("Fishing", Priority = 0), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<PlanFishingData> fishing;
    [ListDrawer("Messages", Priority = 0), NewLineFieldDrawer("0/user_name", GetNameFromContent = true, Priority = 1)]
    public List<PlanMessageData> messages;
    [ListDrawer("Restrictions", Priority = 0), NewLineFieldDrawer("0/user_name", GetNameFromContent = true, Priority = 1)]
    public List<PlanRestrictionData> restriction_settings;
}

[Serializable]
public class PlanLayerData
{
    [IntFieldDrawer("Layer ID")]
    public int layer_id;
    [ReferenceDropdownDrawer("Layer Name", "meta.layer_name")]
    public string name;
    [StringFieldDrawer("Layer Editing Type")]
    public string layer_editing_type;
    [ListDrawer("Geometry", Priority = 0), NewLineFieldDrawer("0/geometry_id", GetNameFromContent = true, Priority = 1)]
    public List<PlanLayerGeometryData> geometry;
    [ListDrawer("Warnings", Priority = 0), NewLineFieldDrawer("0/geometry_id", GetNameFromContent = true, Priority = 1)]
    public List<PlanGeomWarningData> warnings;
    [ListDrawer("Deleted", Priority = 0), NewLineFieldDrawer("0/geometry_id", GetNameFromContent = true, Priority = 1)]
    public List<PlanGeomDeletedData> deleted;
}

[Serializable]
public class PlanLayerGeometryData
{
    [IntFieldDrawer("Geometry ID")]
    public int geometry_id;
    [StringFieldDrawer("FID")]
    public string FID;
    [IntFieldDrawer("Persistent Geometry ID")]
    public int geometry_persistent;
    public string geometry; //float[][]
    [DictionaryFieldDrawer("Data"), StringFieldDrawer(null, Priority = 1), StringFieldDrawer(null, Priority = 2)]
    public Dictionary<string, string> data;
    [IntFieldDrawer("Country ID")]
    public int? country;
    [StringFieldDrawer("Layer Type")]
    public string type; //multitype has csv int
    [InlineFieldDrawer("Geometry Info")]
    public BaseGeometryInfoData base_geometry_info;
    [InlineFieldDrawer("Cable Info")]
    public EnergyCableData cable;
    [ListDrawer("Energy", Priority = 0), NewLineFieldDrawer("0/maxcapacity", GetNameFromContent = true, Priority = 1, InfoText = "")]
    public List<EnergyOutputData> energy_output;
}

[Serializable]
public class BaseGeometryInfoData
{
    [IntFieldDrawer("Geometry ID")]
    public int geometry_id;
    [IntFieldDrawer("MSP ID")]
    public int geometry_mspid;
    [IntFieldDrawer("Persistent ID")]
    public int geometry_persistent;
}

[Serializable]
public class BaseGridGeometryInfoData
{
    [IntFieldDrawer("Geometry ID")]
    public int geometry_id;
    [IntFieldDrawer("Persistent ID")]
    public int geometry_persistent;
}

[Serializable]
public class EnergyOutputData
{
    [LongFieldDrawer("Maximum Capacity")]
    public long maxcapacity;
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Active")]
    public bool active;
}

[Serializable]
public class EnergyCableData
{
    [InlineFieldDrawer("Energy Connection Start")]
    public EnergyCableConnection start;
    [InlineFieldDrawer("Energy Connection Start")]
    public EnergyCableConnection end;
    [StringFieldDrawer("Coordinates")]
    public string coordinates;
}

[Serializable]
public class EnergyCableConnection
{
    [IntFieldDrawer("Geometry ID")]
    public int geometry_id;
    [IntFieldDrawer("Geometry Persistent")]
    public int geometry_persistent;
}



[Serializable]
public class PlanGeomWarningData
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Issue Type")]
    public EPlanRestrictionType issue_type;
    [FloatFieldDrawer("x")]
    public float x;
    [FloatFieldDrawer("y")]
    public float y;
    [IntFieldDrawer("Plan ID")]
    public int source_plan_id;
    [StringFieldDrawer("Message")]
    public string restriction_message;
}

[Serializable]
public class PlanGeomDeletedData
{
    [IntFieldDrawer("Geometry ID")]
    public int geometry_id;
    [IntFieldDrawer("Geometry MSP ID")]
    public int geometry_mspid;
    [InlineFieldDrawer("Geometry Info")]
    public BaseGeometryInfoData base_geometry_info;
}

[Serializable]
public class PlanGridData
{
    [IntFieldDrawer("Grid ID")]
    public int grid_id;
    [IntFieldDrawer("Persistent Grid ID")]
    public int grid_persistent;
    [StringFieldDrawer("Grid Name")]
    public string name;
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Active")]
    public bool active;
    [ListDrawer("Energy Countries", Priority = 0), NewLineFieldDrawer("0/country", GetNameFromContent = true, Priority = 1, InfoText = "")]
    public List<PlanGridDistributionData> energy;
    [ListDrawer("Removed", Priority = 0), NewLineFieldDrawer("Geometry", Priority = 1)]
    public List<PlanGridSourceSocketData> removed;
    [ListDrawer("Sockets", Priority = 0), NewLineFieldDrawer("Geometry", Priority = 1)]
    public List<PlanGridSourceSocketData> sockets;
    [ListDrawer("Sources", Priority = 0), NewLineFieldDrawer("Geometry", Priority = 1)]
    public List<PlanGridSourceSocketData> sources;
}

[Serializable]
public class PlanGridDistributionData
{
    [IntFieldDrawer("Country ID")]
    public int country;
    [LongFieldDrawer("Expected Energy")]
    public long expected;
}

[Serializable]
public class PlanGridSourceSocketData
{
    [InlineFieldDrawer("Grid Geometry Info")]
    public BaseGridGeometryInfoData geometry;
}

[Serializable]
public class PlanMessageData
{
    [IntFieldDrawer("Country ID")]
    public int country_id;
    [StringFieldDrawer("User Name")]
    public string user_name;
    [StringFieldDrawer("Message")]
    public string text;
    [DoubleFieldDrawer("Time")]
    public double time;
}

[Serializable]
public class PlanFishingData
{
    [IntFieldDrawer("Fishing ID")]
    public int fishing_id;
    [IntFieldDrawer("Country ID")]
    public int fishing_country_id;
    [IntFieldDrawer("Plan ID")]
    public int fishing_plan_id;
    [StringFieldDrawer("Type")]
    public string fishing_type;
    [FloatFieldDrawer("Amount")]
    public float fishing_amount;
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Active")]
    public bool fishing_active;
}

[Serializable]
public class PlanRestrictionData
{
    [IntFieldDrawer("Country ID")]
    public int country_id;
    [IntFieldDrawer("Entity Type ID")]
    public int entity_type_id;
    [FloatFieldDrawer("Size")]
    public float size;
    [StringFieldDrawer("Layer Name")]
    public string layer_name;
}


