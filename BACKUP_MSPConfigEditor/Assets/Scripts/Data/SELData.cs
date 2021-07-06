using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[Serializable]
public class SELData
{
    [ListDrawer("Shipping Layers", Priority = 0, InfoText = "Layers containing shipping lanes used within the shipping model. At least one layer must be defined for the model to function."), ReferenceDropdownDrawer("Layer", "meta.layer_name")]
    public List<string> shipping_lane_layers;
    [ReferenceDropdownDrawer("Country Border layer", "meta.layer_name", InfoText = "Layer used as country borders (EEZ), this is used to determine the country a port belongs to to calculate KPI values.")]
    public string country_border_layer;

    [IntFieldDrawer("Point merge distance", InfoText = "Distance at which shipping lanes points will be merged to optimize the shipping model")]
    public int shipping_lane_point_merge_distance;
    [IntFieldDrawer("Subdivide distance", InfoText = "Length at which shipping lanes will be subdivide to improve accuracy of the shipping model")]
    public int shipping_lane_subdivide_distance;
    [DoubleFieldDrawer("Implicit lane distance limit", InfoText = "Maximum distance used to create implicit edges (invisible lanes between defined shipping lane layers")]
    public double shipping_lane_implicit_distance_limit;

    [ListDrawer("Port Layers", Priority = 0, InfoText = "Layers containing the ports used within the shipping model"), NewLineFieldDrawer("0/layer_name", Priority = 1, GetNameFromContent = true)]
    public List<SELPortData> port_layers;
    [ListDrawer("Port Intensity", Priority = 0, InfoText = "Predefined intensity levels of all ship types at the defined ports for the different moments in time"), NewLineFieldDrawer("0/port_id", Priority = 1, GetNameFromContent = true)]
    public List<SELPortIntensityData> port_intensity;

    [NewLineFieldDrawer("Maintenance destinations", InfoText = "Settings for layers requiring maintenance shipping vessels")]
    public SELMaintenanceIntensityData maintenance_destinations;
    [ListDrawer("Configured Routes", Priority = 0, InfoText = "Percentage based intensity from one to another port, distinguishing ship type,  "), NewLineFieldDrawer("0/source_port_id", Priority = 1, GetNameFromContent = true)]
    public List<SELConfiguredRoute> configured_routes;
    [ListDrawer("Restriction Layer Exceptions", Priority = 0, InfoText = "Settings for restricted layers where specific ship types are allowed to move through"), NewLineFieldDrawer("0/layer_name", Priority = 1, GetNameFromContent = true)]
    public List<SELRestrictionLayerExceptionsData> restriction_layer_exceptions;
    [ListDrawer("Restriction Group Mapping", Priority = 0, InfoText = "Defines the groups of ship which are send via the layer types with the indicated name"), InlineFieldDrawer("0/layer_name", Priority = 1, GetNameFromContent = true)]
    public List<SELLayerTypeRestrictionMapping> layer_type_restriction_group_mapping;
    [NewLineFieldDrawer("Output configuration", InfoText = "Settings for the raster file exported by SEL")]
    public SELOutputConfigData output_configuration;
    [NewLineFieldDrawer("Risk heatmap settings", InfoText = "Settings for the risk levels indicated on the risk raster file exported by SEL")]
    public SELRiskHeatmapData risk_heatmap_settings;
    [ListDrawer("Heatmap settings", Priority = 0, InfoText = "Settings for the shipping intensity raster file exported by SEL"), NewLineFieldDrawer("0/layer_name", GetNameFromContent = true, Priority = 1)]
    public List<SELHeatmapSettingData> heatmap_settings;
    [NewLineFieldDrawer("Heatmap bleed settings", InfoText = "Settings for the bleed levels on the shipping intensity raster file exported by SEL")]
    public SELHeatmapBleedData heatmap_bleed_config;
    [ListDrawer("Ship types", Priority = 0, InfoText = "All ship types available within the shipping model"), NewLineFieldDrawer("0/ship_type_name", GetNameFromContent = true, Priority = 1)]
    public List<SELShipType> ship_types;
    [ListDrawer("Ship restriction groups", Priority = 0, InfoText = "Settings restriction groups utilizing the ship types defined in the shipping model"), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<SELShipRestrictionGroupData> ship_restriction_groups;
    [ListDrawer("Layer Type Ship Type Mapping", Priority = 0), NewLineFieldDrawer("0/layer_type", GetNameFromContent = true, Priority = 1)]
    public List<SELLayerTypeShipTypeMapping> layer_type_ship_type_mapping;
    [ListDrawer("KPI Configuration", Priority = 0), NewLineFieldDrawer("0/categoryName", GetNameFromContent = true, Priority = 1)]
    public List<SELKPICategoryData> shipping_kpi_config;
}

[Serializable]
public class SELPortData
{
    public enum PortType { DefinedPort, MaintenanceDestination }
    [ReferenceDropdownDrawer("Layer Name", "meta.layer_name", InfoText = "Name of the layer set as a shipping destination")]
    public string layer_name;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Port Type", InfoText = "Define whether the port is defined as a shipping destination, or a maintenance destination")]
    public PortType port_type;
}

[Serializable]
public class SELPortIntensityData
{
    [StringFieldDrawer("Port ID", InfoText = "Name of the port set as a shipping destination")]
    public string port_id;
    [ListDrawer("Ship intensity values", Priority = 0, InfoText = "Amount of a specific type of ship occupying the indicated port at the given month in-game"), NewLineFieldDrawer("0/ship_type_id", GetNameFromContent = true, Priority = 1)]
    public List<SELShipIntensityData> ship_intensity_values;
}

[Serializable]
public class SELShipIntensityData
{
    [IntFieldDrawer("Starting time", InfoText = "In-Game month at which the port contains this amount of ship, -1 meaning that the intensity level is set before start of the game")]
    public int start_time;
    [IntFieldDrawer("Ship type ID", InfoText = "ID of the type of ship")]
    public int ship_type_id;
    [IntFieldDrawer("Ship intensity", InfoText = "Amount of ship from the indicated ID")]
    public int ship_intensity;
}

[Serializable]
public class SELMaintenanceIntensityData
{
    static SELMaintenanceIntensityData()
    {
        //ConstraintManager.RegisterConstraint(typeof(SELMaintenanceIntensityData), nameof(construction_intensity_multiplier), 0, new MinConstraint<float>(5, true, EConstraintType.Error));
        //ConstraintManager.RegisterConstraint(typeof(SELMaintenanceIntensityData), nameof(base_intensity_per_square_km), 0, new MinConstraintRef<float>(new RefVar<float>("1/point_construction_intensity"), true, EConstraintType.Error, "point intensity"));
        //ConstraintManager.RegisterConstraint(typeof(SELMaintenanceIntensityData), nameof(base_intensity_per_square_km), 0, new RangeConstraint<float>(10f, 20f, EConstraintType.Warning));
        //ConstraintManager.RegisterConstraint(typeof(SELMaintenanceIntensityData), nameof(base_intensity_per_square_km), 0, new RangeConstraintRef<float>(new RefVar<float>("1/construction_intensity_multiplier"), new RefVar<float>("1/point_construction_intensity"), EConstraintType.Warning, "Intensity multiplier", "Construction intensity"));
    }

    [FloatSliderFieldDrawer("Construction intensity multiplier", -10f, 10f, InfoText = "Multiplicationfactor by which the maintenance intensity is multiplied during construction")]
    public float construction_intensity_multiplier;
    [FloatSliderFieldDrawer("Base intensity / km2", 0, 100f, InfoText = "Default intensity per square meter for polygon based maintenance layers")]
    public float base_intensity_per_square_km;
    [FloatSliderFieldDrawer("Point construction intensity", 0, 100f, InfoText = "Default amount of ship navigation to every point based maintenance layers during construction")]
    public float point_construction_intensity;
    [FloatSliderFieldDrawer("Point intensity", 0, 100f, InfoText = "Default intensity per point for point based maintenance layers")]
    public float point_intensity;
}

[Serializable]
public class SELConfiguredRoute
{
    [StringFieldDrawer("Source Port ID", InfoText = "Name of the starting port for the defined route")]
    public string source_port_id;
    [StringFieldDrawer("Destination Port ID", InfoText = "Name of the ending port for the defined route")]
    public string destination_port_id;
    [IntFieldDrawer("Ship Type ID", InfoText = "ID of the ship type send over the defined route")]
    public int ship_type_id;
    [FloatFieldDrawer("Intensity Percentage", InfoText = "Percentage of ship in the source port of the defined ID send over the route")]
    public float intensity_percentage;
    [IntFieldDrawer("Starting Time", InfoText = "Month in In-Game time in which the defined route starts")]
    public int start_time;
}

[Serializable]
public class SELRestrictionLayerExceptionsData
{
    static SELRestrictionLayerExceptionsData()
    {
        ConstraintManager.RegisterConstraint(typeof(SELRestrictionLayerExceptionsData), nameof(layer_name), 0, new RegexConstraint(@"\b\s?\b", EConstraintType.Error, "Regex test failed"));
    }
    [ReferenceDropdownDrawer("Layer Name", "meta.layer_name", InfoText = "Name of the layer used for the restriction exception")]
    public string layer_name;
    [ListDrawer("Layer types", InfoText = "Types within the layer which are excluded, leaving this on null includes all types in the exclusion"), StringFieldDrawer(null, Priority = 1)]
    public List<string> layer_types;
    [ListDrawer("Allowed Ships", Priority = 0, InfoText = "Ship types added to the exclusion"), NewLineFieldDrawer("Ship", Priority = 1)]
    public List<SELAllowedShipData> allowed_ships;
}

[Serializable]
public class SELAllowedShipData
{
    [ListDrawer("Ship type ids", InfoText = "ID of the ship types which are excluded from the restriction"), IntFieldDrawer(null, Priority = 1)]
    public List<int> ship_type_ids;
    [FloatFieldDrawer("Multiplication factor", InfoText = "Multiplication factor of the cost of sailing over the associated layer & types")]
    public float cost_multiplier;
}

[Serializable]
public class SELLayerTypeRestrictionMapping
{
    [StringFieldDrawer("Layer type", InfoText = "Layer type used to map the restriction groups to")]
    public string layer_type;
    [ListDrawer("Restriction groups", InfoText = "Restriction group ids mapped to the indicated layer types"), IntFieldDrawer(null, Priority = 1)]
    public List<int> restriction_groups;
}

[Serializable]
public class SELLayerTypeShipTypeMapping
{
    public string layer_type;
    public List<int> ship_type_ids;
}

[Serializable]
public class SELOutputConfigData
{
    [IntFieldDrawer("Pixels per MEL cell", InfoText = "Scale of a MEL cell in the raster files")]
    public int pixels_per_mel_cell;
    [IntFieldDrawer("Simulation area cell size", InfoText = "Size of a cell in the simulation area")]
    public int simulation_area_cell_size;
}

[Serializable]
public class SELRiskHeatmapData
{
    [ListDrawer("Restriction layer exceptions", InfoText = "Layers excluded from the Risk Heatmaps even if they are defined in the restriction matrix"), StringFieldDrawer(null, Priority = 1)]
    public List<string> restriction_layer_exceptions;
    [IntFieldDrawer("Shipping intensity risk value", InfoText = "From this intensity the cell should be marked as a risk zone. So any cells that have a shipping intensity that exceeds this value will be plotted on the resulting risk heatmap")]
    public int shipping_intensity_risk_value;
}

[Serializable]
public class SELHeatmapSettingData
{
    public enum HeatmapType { ShippingIntensity, Riskmap }

    [BoolFieldDrawer("Output for MEL", InfoText = "Identifies whether this heatmap is used as a pressure in MEL")]
    public bool output_for_mel;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Heatmap type", InfoText = "Is ShippingIntensity by default, yet can be set to Riskmap to output a riskmap")]
    public HeatmapType heatmap_type;
    //[StringFieldDrawer("Layer name", InfoText = "Name of the layer used to represent the heatmap in the MSP Challenge")]
    [ReferenceDropdownDrawer("Layer Name", "meta.layer_name", InfoText = "Name of the layer used to represent the heatmap in the MSP Challenge")]
    public string layer_name;
    [ListDrawer("Ship type ids", InfoText = "IDs of the ship types associated to the heatmap"), IntFieldDrawer(null, Priority = 1)]
    public List<int> ship_type_ids;
    [ListDrawer("Heatmap range", Priority = 0, InfoText = "Range of strengths at which the heatmap is shown, linear interpolation between the ranges is used to create a fluent map"), NewLineFieldDrawer("0/input", GetNameFromContent = true, Priority = 1)]
    public List<SELHeatmapRangeData> heatmap_range;
}

[Serializable]
public class SELHeatmapRangeData
{
    [IntFieldDrawer("Input", InfoText = "Amount of ship required to display the output intensity on the heatmap")]
    public int input;
    [FloatSliderFieldDrawer("Output", 0, 1, InfoText = "Output color (0-1) on the heatmap")]
    public float output;
}

[Serializable]
public class SELHeatmapBleedData
{
    [IntFieldDrawer("Treshold", InfoText = "From what values the bleeding effect should start. This value value represents a maximum intensity on a single pixel. Any raster values that are over bleed_treshold bleed over to neighbouring edges.")]
    public int bleed_treshold;
    [FloatFieldDrawer("Overflow curve power", InfoText = "The power of the the function that selects the bleeding kernel.")]
    public float bleed_overflow_curve_power;
    [FloatFieldDrawer("Overflow curve multiplier", InfoText = "The multiplier applied to the function that selects the bleeding kernel.")]
    public float bleed_overflow_curve_multiplier;
    [IntFieldDrawer("Number of kernels", InfoText = "The number of bleeding kernels we generate. Each bleeding kernel will cause a bleed over a larger area, and the size of the bleeding is limited by the biggest kernel")]
    public int bleed_number_of_kernels;
}

[Serializable]
public class SELShipType
{
    public enum ShippingRouteType { RegularShipping, Maintenance }

    [IntFieldDrawer("Type ID", InfoText = "Defines the shit type")]
    public int ship_type_id;
    [StringFieldDrawer("Type name", InfoText = "Defines the shit type name")]
    public string ship_type_name;
    [StringFieldDrawer("Restriction group", InfoText = "Defines the restriction group the ship type is part of")]
    public string ship_restriction_group;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Routing type", InfoText = "Defines how the ships in this type are routed")]
    public ShippingRouteType ship_routing_type;
    [FloatSliderFieldDrawer("Agility", 0, 1, InfoText = "Defines how willing the ship type is to take shortcuts")]
    public float ship_agility;
}

[Serializable]
public class SELShipRestrictionGroupData
{
    [IntFieldDrawer("Group ID", InfoText = "Number of the restriction group")]
    public int group_id;
    [StringFieldDrawer("Group name", InfoText = "Name of the restriction group")]
    public string name;
}

[Serializable]
public class SELKPICategoryData
{
    public enum ESELCategoryValueType { Sum, Average, Manual }
    [StringFieldDrawer("Name")]
    public string categoryName;
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Colour")]
    public Color categoryColor;
    [StringFieldDrawer("Unit")]
    public string unit;
    [StringFieldDrawer("Generated Values per Port")]
    public string generateValuesPerPort;
    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Category Value Type")]
    public ESELCategoryValueType categoryValueType;
    [ListDrawer("Value Definitions"), NewLineFieldDrawer("0/valueName", GetNameFromContent = true, Priority = 1)]
    public List<SELKPIValueDefinitionData> valueDefinitions;
}

[Serializable]
public class SELKPIValueDefinitionData
{
    [StringFieldDrawer("Name")]
    public string valueName;
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Colour")]
    public Color valueColor;
    [StringFieldDrawer("Unit")]
    public string unit;
    [IntFieldDrawer("Country colour display", InfoText = "ID of country that this specific KPI targets, 0 for global (no country specific color when filtering), -1 for country specific generated by client (each country will get this with coloring specific to the filter). Defaults to -1 to keep consistency.")]
	public int valueDependentCountry = -1;
}

