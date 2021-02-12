using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.UI;

public enum ELayerEditingType { none, cable, transformer, socket, sourcepoint, sourcepolygon, sourcepolygonpoint, multitype, protection }
public enum ELayerSpecialEntityType { Default, ShippingLine }
public enum EPointVisuals { anchor, angling, aquaculturealgea, aquaculturefish, aquacultureshellfish, batteryempty, batteryfull, birdwatching, canoeing, chatch, city, compass, directionality, diving, dot, dredging, dump, gasbarrel, info, lighthouse, lightning, oilbarrel, oilrig, oilrigclosed, pin, port2, rowing, ship, socket, steeringwheel, sunbathing, surfing, transformer, trashcan, walking, wreck }
public enum ERegions { northsee, balticline, simcelt, adriatic }
public enum EGeoTypes { polygon, point, line, raster }
public enum ECategories { management, activities, ecology, dynamic_layers}
public enum ESubcategories { aquaculture, birds, birds_and_mammals, biodiversity_indicator, cables_and_pipelines, dredging, energy, environmental_conditions, fish, fishing, governance, habitats, macrobenthos, mammals, natural_resources, pressure, protected_areas, recreational, restricted_zones, sensitive_areas, shipping, shipping_activity, shipping_infrastructure, telecommunications, temporary_restricted_areas, transportation_infrastructure}
public enum ERasterColorInterpolationMode { Linear, Point}
public enum ERasterMaterial { RasterBathymetry, RasterMELNew, RasterMELOld, RasterMELCustomizable }
public enum ERasterPattern { Default, MEL_pattern, MEL_pattern_dense, MEL_patten_dense2 }

[Serializable]
public class MetaData
{
    //General Settings
    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("General Settings", 18, Priority = 1)]
    [StringFieldDrawer("Name", InfoText = "Name of the layer file as uploaded on GeoServer")]
    public string layer_name;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Geotype", InfoText = "The type of geometry the data layer consists of")]
    public EGeoTypes layer_geotype;
    [StringFieldDrawer("Short", InfoText = "The name of the layer as shown inside the game")]
    public string layer_short;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Layer category", InfoText = "The category this layer is part of")]
    public ECategories layer_category = ECategories.activities;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Layer subcategory", InfoText = "The sub-category this layer is part of")]
    public ESubcategories layer_subcategory = ESubcategories.governance;
    [JsonConverter(typeof(Bool01Converter))]
    [BoolFieldDrawer("Download from Geoserver", Priority = 1)]
    public bool layer_download_from_geoserver;

    //Raster Settings
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Raster Settings",18, Priority = 1)]
    [IntFieldDrawer("Raster Width")]
    public int layer_width;
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [IntFieldDrawer("Raster Height")]
    public int layer_height;
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Raster Material", InfoText = "The material used to render this raster")]
    public ERasterMaterial layer_raster_material = ERasterMaterial.RasterMELCustomizable;
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [DropdownFieldDrawer("Filter Mode", InfoText = "")]
    public FilterMode layer_raster_filter_mode = FilterMode.Bilinear;
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [DropdownFieldDrawer("Color Interpolation Mode", InfoText = "")]
    public ERasterColorInterpolationMode layer_raster_color_interpolation = ERasterColorInterpolationMode.Linear;
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Raster Pattern", InfoText = "The pattern used to render this raster")]
    public ERasterPattern layer_raster_pattern = ERasterPattern.Default;
    [HideIfValue("1/layer_geotype", EGeoTypes.raster, true)]
    [FloatFieldDrawer("Raster Minimum Cutoff Value")]
    public float layer_raster_minimum_value_cutoff = 0.05f;

    //Interaction Settings
    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Interaction Settings", 18, Priority = 1)]
    [JsonConverter(typeof(Bool01Converter))]
    [BoolFieldDrawer("Active", Priority = 1)]
    public bool layer_active;
    [JsonConverter(typeof(Bool01Converter))]
    [BoolFieldDrawer("Selectable", Priority = 1)]
    public bool layer_selectable;
    [JsonConverter(typeof(Bool01Converter))]
    [BoolFieldDrawer("Editable", Priority = 1)]
    public bool layer_editable;
    [JsonConverter(typeof(Bool01Converter))]
    [BoolFieldDrawer("Toggleable", Priority = 1)]
    public bool layer_toggleable;
    [JsonConverter(typeof(Bool01Converter))]
    [BoolFieldDrawer("Active on Start", Priority = 1)]
    public bool layer_active_on_start;
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Green", Priority = 1)]
    public bool layer_green;

    //Info Settings
    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Info Settings", 18, Priority = 1)]
    [StringFieldDrawer("Tooltip")]
    public string layer_tooltip;
    [StringFieldDrawer("Information")]
    public string layer_information;
    [StringFieldDrawer("Media Link")]
    public string layer_media;

    [NewLineFieldDrawer("Layer Text Info")]
    public LayerTextInfoData layer_text_info;


    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("States", 18, Priority = 1)]
    [ListDrawer("States", Priority = 0), NewLineFieldDrawer("0/state", GetNameFromContent = true, Priority = 1, InfoText = "")]
    public List<LayerStateData> layer_states = new List<LayerStateData>();

    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Editing Types", 18, Priority = 1)]
    [JsonConverter(typeof(EditingTypeConverter)), DropdownFieldDrawer("Editing Type")]
    public ELayerEditingType layer_editing_type;
    [JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Special Entity Type")]
    public ELayerSpecialEntityType layer_special_entity_type;
    [IntFieldDrawer("Depth", InfoText = "Layering position, the higher the number the further on top the layer is placed (2 = in front of 1)")]
    public int layer_depth;

    [JsonConverter(typeof(LayerTypeConverter))]
    [DictionaryFieldDrawer("Layer Type", AutoIncrement = true, AutoIncrementFillGaps = true),
        IntFieldDrawer(null, Priority = 1, Nullable = false),
        NewLineFieldDrawer("0/displayName", GetNameFromContent = true, Priority = 2)]
    public Dictionary<int,LayerTypeData> layer_type;
    [ListDrawer("Info Properties", Priority = 0), NewLineFieldDrawer("0/property_name", GetNameFromContent = true, Priority = 1, InfoText = "")]
    public List<LayerInfoPropertiesData> layer_info_properties;

}

[Serializable]
public class LayerTextInfoData
{
    
    public enum ETextSize
    {
        XS = 0,
        S = 1,
        M = 2,
        L = 3,
        XL = 4
    }

    public enum ETextState
    {
        Current = 0,
        View = 1,
        Edit = 2
    }

    [DictionaryFieldDrawer("Text State"),
       DropdownFieldDrawer(null, Priority = 1), 
        StringFieldDrawer(null, Priority = 2, Nullable = false)]
    public Dictionary<ETextState, string> property_per_state;
    [JsonConverter(typeof(ColourHexConverter))]
    public Color text_color;       //Colour picker

    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Text Size")]
    public ETextSize text_size;    //Dropdown
    [FloatFieldDrawer("Zoom % Cutoff")]
    public float zoom_cutoff;
    [FloatFieldDrawer("X Offset")]
    public float x;
    [FloatFieldDrawer("Y Offset")]
    public float y;
    [FloatFieldDrawer("Z Offset")]
    public float z;
    
}

[Serializable]
public class LayerStateData
{
    public enum ELayerState { ASSEMBLY, ACTIVE, DISMANTLE }

    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("State")]
    public ELayerState state = ELayerState.ASSEMBLY;
    [IntFieldDrawer("Time")]
    public int time = 0;
}

[Serializable]
public class LayerTypeData
{
    public enum EApprovalType { NotDependent, EEZ, AllCountries, AreaManager }
    public enum ELinePatterns { Solid, ShortDash, LongDash }
    public enum EPolygonPatterns { Solid = 0, Diagonal = 1, Horizontal2 = 2, Horizontal3 = 3, Horizontal4 = 4, Horizontal5 = 5, Dots = 6, Anchor = 7, NoShip = 8, Military = 9, Wave = 10, WindSpeed = 11, WindFarm = 12, DiagonalWave = 13, TidalFarm = 14, Current = 15, FineMesh = 16, Sailing = 17, Bird = 18, WaveSpeed = 19, Bone = 20, Cog = 21, Squiggle = 22, MEL_pattern, MEL_pattern_dense, MEL_patten_dense2 }
    public enum EPointPatterns { None, Anchor, Angling, Aquaculturealgea, Aquaculturefish, Aquacultureshellfish, Batteryempty, Batteryfull, Birdwatching, Canoeing, Chatch, City, Compass, Directionality, Diving, Dot, Dredging, Dump, Gasbarrel, Info, Lighthouse, Lighting, Oilbarrel, Oilrig, Oilrigclosed, Pin, Port2, Rowing, Ship, Socket, Steeringwheel, Sunbathing, Surfing, Transformer, Trashcan, Walking, Wreck }



    [StringFieldDrawer("Name")]
    public string displayName;
    [JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Approval")]
    public EApprovalType approval;
    [IntFieldDrawer("Value")]
    public int value;


    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Polygon Visuals", 18, Priority = 1)]
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Show Polygons")]
    public bool displayPolygon;
    [HideIfValue("1/displayPolygon", false)]
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Color")]
    public Color polygonColor;
    [HideIfValue("1/displayPolygon", false)]
    //CANNOT CHANGE THIS TO A DROPDOWN UNTILL THEY ARE PROPER STRINGS, AND REPLACED IN THE CONFIG, OR NULL VALUES CAN BE HANDLED
    //[JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Pattern Name")]
    //public EPolygonPatterns polygonPatternName = EPolygonPatterns.Solid;
    [StringFieldDrawer("Pattern Name")]
    public string polygonPatternName;


    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Inner Glow", 18, Priority = 1)]
    [JsonConverter(typeof(Bool01Converter)),BoolFieldDrawer("Show Inner Glow")]
    public bool innerGlowEnabled;
    [HideIfValue("1/innerGlowEnabled", false)]
    [IntFieldDrawer("Radius")]
    public int innerGlowRadius;
    [HideIfValue("1/innerGlowEnabled", false)]
    [IntFieldDrawer("Iterations")]
    public int innerGlowIterations;
    [HideIfValue("1/innerGlowEnabled", false)]
    [FloatFieldDrawer("Multiplier")]
    public float innerGlowMultiplier;
    [HideIfValue("1/innerGlowEnabled", false)]
    [FloatFieldDrawer("Pixel Size")]
    public float innerGlowPixelSize;

    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Line Visuals", 18, Priority = 1)]
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Show Lines")]
    public bool displayLines;
    [HideIfValue("1/displayLines", false)]
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Color")]
    public Color lineColor;
    [HideIfValue("1/displayLines", false)]
    [FloatFieldDrawer("Width")]
    public float lineWidth;
    [HideIfValue("1/displayLines", false)]
    [StringFieldDrawer("Icon")]
    public string lineIcon;
    [HideIfValue("1/displayLines", false)]
    [JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Type")]
    public ELinePatterns linePatternType;

    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Point Visuals", 18, Priority = 1)]
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Show Points")]
    public bool displayPoints;
    [HideIfValue("1/displayPoints", false)]
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Color")]
    public Color pointColor = Color.black;
    [HideIfValue("1/displayPoints", false)]
    [FloatFieldDrawer("Size")]
    public float pointSize = 0.03f;
    [HideIfValue("1/displayPoints", false)]
    [JsonConverter(typeof(StringEnumConverter)), DropdownFieldDrawer("Sprite Name")]
    public EPointPatterns pointSpriteName;

    [LineSpacer(1f, Priority = 1)]
    [TitleSpacer("Additional Information", 18, Priority = 1)]
    [StringFieldDrawer("Description")]
    public string description;
    [LongFieldDrawer("Capacity")]
    public long capacity;
    [FloatFieldDrawer("Cost")]
    public float investmentCost;
    [IntFieldDrawer("Availability")]
    public int availability;
    [StringFieldDrawer("Media Link")]
    public string media;
}

[Serializable]
public class LayerInfoPropertiesData
{
    public enum ContentValidation { None, ShippingWidth, NumberCables }

    [StringFieldDrawer("Name")]
    public string property_name;
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Enabled")]
    public bool enabled;
    [JsonConverter(typeof(Bool01Converter)), BoolFieldDrawer("Editable")]
    public bool editable;
    [StringFieldDrawer("Display Name")]
    public string display_name;
    [StringFieldDrawer("Sprite Name")]
    public string sprite_name;

    [BoolFieldDrawer("Update Visuals")]
    public bool update_visuals;

    [BoolFieldDrawer("Update Text")]
    public bool update_text;

    [BoolFieldDrawer("Update Calculation")]
    public bool update_calculation;

    [DropdownFieldDrawer("Content Type")]
    public InputField.ContentType content_type;

    [DropdownFieldDrawer("Content Validation")]
    public ContentValidation content_validation;

    [StringFieldDrawer("Unit")]
    public string unit;
    [StringFieldDrawer("Default Value")]
    public string default_value;
}

//public class RasterObject
//{
//    public enum ERasterColorInterpolationMode
//    {
//        Linear,
//        Point
//    }

//    public string url;
//    public bool request_from_server;
//    public List<List<float>> boundingbox;
//    public string layer_raster_material;
//    public string layer_raster_pattern;
//    public float layer_raster_minimum_value_cutoff;
//    [JsonConverter(typeof(StringEnumConverter))]
//    public ERasterColorInterpolationMode layer_raster_color_interpolation;
//}