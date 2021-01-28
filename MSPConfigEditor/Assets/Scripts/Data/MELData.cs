using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;

[Serializable]
public class MELData
{
    [StringFieldDrawer("Model File", InfoText = "The model that should be used in this config file, contact administrators for available files")]
    public string modelfile;
    [IntFieldDrawer("Mode")]
    public int? mode;
    [IntFieldDrawer("Rows", InfoText = "The amount of cell rows in the MEL raster files")]
    public int rows;
    [IntFieldDrawer("Columns", InfoText = "The amount of cell columns in the MEL raster files")]
    public int columns;
    [IntFieldDrawer("Cellsize", InfoText = "The size in KM of the cells")]
    public int cellsize;
    [IntFieldDrawer("X Minimum Position", InfoText = "Minimum position on the x axis in world position using EPSG:3035 (ETRS89 / ETRS-LAEA)")]
    public int x_min;
    [IntFieldDrawer("Y Minimum Position", InfoText = "Minimum position on the y axis in world position using EPSG:3035 (ETRS89 / ETRS-LAEA)")]
    public int y_min;
    [IntFieldDrawer("X Maximum Position", InfoText = "Maximum position on the x axis in world position using EPSG:3035 (ETRS89 / ETRS-LAEA)")]
    public int x_max;
    [IntFieldDrawer("Y Maximum Position", InfoText = "Maximum position on the y axis in world position using EPSG:3035 (ETRS89 / ETRS-LAEA)")]
    public int y_max;
    [FloatSliderFieldDrawer("Initial fishing mapping", 0, 1)]
    public float initialFishingMapping;
    [IntSliderFieldDrawer("Fishing Display Scale", 0, 100, InfoText = "The range of which fishing raster files are displayed in percentages")]
    public int fishingDisplayScale;
    [ListDrawer("Pressures", Priority = 0, InfoText = "Pressures associated to the ecological model impacting the environment"), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<MELPressureData> pressures;
    [ListDrawer("Fishing", Priority = 0, InfoText = "Distribution of fishing for varying fishing types in the seabasin"), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<MELFishingData> fishing;
    [ListDrawer("Outcomes", Priority = 0, InfoText = "Settings for outcomes of MEL / Ecopath with Ecosim fed into the game"), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<MELOutcomeData> outcomes;
    [ListDrawer("Ecology Categories", Priority = 0), NewLineFieldDrawer("0/categoryName", GetNameFromContent = true, Priority = 1, InfoText = "Settings for the visualization of ecology categories and values of MEL inside the KPI graphs")]
    public List<MELEcologyCategoryData> ecologyCategories;

}

[Serializable]
public class MELPressureData
{
    [StringFieldDrawer("Pressure Name", InfoText = "Name of the pressure")]
    public string name;
    [ListDrawer("Pressure Layers", Priority = 0, InfoText = "Layers impacting the specified pressure"), NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<MELPressureLayerData> layers;
}

[Serializable]
public class MELPressureLayerData
{
    //[StringFieldDrawer("Layer Name", InfoText = "Name of the layer impacting the pressure")]
    [ReferenceDropdownDrawer("Layer Name", "meta.layer_name")]
    public string name;
    [FloatSliderFieldDrawer("Layer Influence", 0, 1, InfoText = "The level of influence this layer has on the pressure at the position of the geometry")]
    public float influence;
    [BoolFieldDrawer("When under construction", InfoText = "Select when the provided pressure levels only count when the layer is under construction")]
    public bool construction;
}

[Serializable]
public class MELFishingData
{
    [StringFieldDrawer("Fishing Type", InfoText = "Name of the fishing type such as 'Bottom Trawl'")]
    public string name;
    [ListDrawer("Fishing Distributions", Priority = 0, InfoText = "Startiong distributions for the specified fishing type per country in the seabasin"), NewLineFieldDrawer("0/country_id", GetNameFromContent = true, Priority = 1)]
    public List<MELFishingCountryData> initialFishingDistribution;
}

[Serializable]
public class MELFishingCountryData
{
    [IntFieldDrawer("Country ID", InfoText = "ID of the country associated to the fishing influence")]
    public int country_id;
    [FloatFieldDrawer("Country Fishing Weight", InfoText = "Amount of fishing by the indicated country")]
    public float weight;
}

[Serializable]
public class MELOutcomeData
{
    [StringFieldDrawer("Outcome Name", InfoText = "Name of the outcome from MEL / Ecosim with Ecopath")]
    public string name;
    [StringFieldDrawer("Outcome Subcategory", InfoText = "Name of the subcategory the outcome should be placed in")]
    public string subcategory;
}

[Serializable]
public class MELEcologyCategoryData
{
    [StringFieldDrawer("Category Name", InfoText = "Name of the category in the KPI graphs")]
    public string categoryName;
    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Category colour", InfoText = "Colour of the category in the KPI graphs")]
    public Color categoryColor;
    [StringFieldDrawer("Category Unit", InfoText = "Unit of the category in the KPI graphs")]
    public string unit;
    [ListDrawer("Category Values", Priority = 0, InfoText = "Values associated to the category"), NewLineFieldDrawer("0/valueName", GetNameFromContent = true, Priority = 1)]
    public List<MELValueDefinitionData> valueDefinitions;
}

[Serializable]
public class MELValueDefinitionData
{
    [StringFieldDrawer("Value Name", InfoText = "Name of the category value")]
    public string valueName;
    [JsonConverter(typeof(ColourHexConverter))]
    public Color valueColor;
    [StringFieldDrawer("Value Unit", InfoText = "Unit of the value in the KPI graphs")]
    public string unit;
    [IntFieldDrawer("Country colour display", InfoText = "ID of country that this specific KPI targets, 0 for global (no country specific color when filtering), -1 for country specific generated by client (each country will get this with coloring specific to the filter). Defaults to -1 to keep consistency.")]
    public int valueDependentCountry = -1;
}

