using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[Serializable]
public class DataModel
{
    static DataModel()
    {
        ConstraintManager.RegisterConstraint(typeof(DataModel), nameof(era_planning_realtime), 0, new MinConstraint<int>(5, true, EConstraintType.Error));
        ConstraintManager.RegisterConstraint(typeof(DataModel), nameof(era_planning_realtime), 0, new MinConstraint<int>(9, true, EConstraintType.Warning));
        //ConstraintManager.RegisterConstraint(typeof(DataModel), nameof(region), 0, new ContainedDictConstraint<string>("1/restrictions", EConstraintType.Warning, "test", true, true));
    }

    //[DictionaryFieldDrawer("Restrictions"),
    //   StringFieldDrawer(null, Priority = 1, Nullable = false),
    //        ListDrawer("Layer restrictions", Priority = 2),
    //        NewLineFieldDrawer("0/startlayer", GetNameFromContent = true, Priority = 3)]
    //   public Dictionary<string, List<RestrictionData>> restrictions;

    [DictionaryFieldDrawer("Restrictions"),
        StringFieldDrawer(null, Priority = 1, Nullable = false),
        ListDrawer("Layer restrictions", Priority = 2),
        NewLineFieldDrawer("0/startlayer", GetNameFromContent = true, Priority = 3)]
    public Dictionary<string, List<RestrictionData>> restrictions;

    [ListDrawer("Plans", Priority = 0),
        NewLineFieldDrawer("0/plan_id", GetNameFromContent = true, Priority = 1)]
    public List<PlanData> plans; 

    [NewLineFieldDrawer("CEL", InfoText = "Config settings relating to the energy simulation setup.")]
    public CELData CEL;

    [NewLineFieldDrawer("SEL")]
    public SELData SEL;

    [NewLineFieldDrawer("MEL")]
    public MELData MEL;

    [ListDrawer("Layers", Priority = 0),
        NewLineFieldDrawer("0/layer_name", GetNameFromContent = true, Priority = 1)]
    public List<MetaData> meta;

    [ListDrawer("Expertise", Priority = 0),
        NewLineFieldDrawer("0/name", GetNameFromContent = true, Priority = 1)]
    public List<ExpertiseData> expertise_definitions;

    [NewLineFieldDrawer("Oceanview")]
    public OceanViewData oceanview;

    [ListDrawer("Objectives", Priority = 0),
        NewLineFieldDrawer("0/country_id", GetNameFromContent = true, Priority = 1)]
    public List<ObjectiveData> objectives;

    [JsonConverter(typeof(StringEnumConverter))]
    [DropdownFieldDrawer("Region")]
    public ERegions region = ERegions.northsee;

    [IntSliderFieldDrawer("Start", 2000, 2080)]
    public int start;

    [IntSliderFieldDrawer("End", 2000, 2080)]
    public int end;

    [IntFieldDrawer("Era total months", InfoText = "Must be a multiplier of 12")]
    public int era_total_months;

    [IntFieldDrawer("Era planning months")]
    public int era_planning_months;

    [IntFieldDrawer("Era planning realtime", InfoText = "Seconds")]
    public int era_planning_realtime;

    [ReferenceDropdownDrawer("Country layer", "meta.layer_name")]
    public string countries;

    [IntFieldDrawer("Min zoom")]
    public int minzoom;

    [IntFieldDrawer("Max zoom")]
    public int maxzoom;

    [StringFieldDrawer("Admin name")]
    public string user_admin_name;

    [StringFieldDrawer("Region manager name")]
    public string user_region_manager_name;

    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Admin colour")]
    public Color user_admin_color;

    [JsonConverter(typeof(ColourHexConverter)), ColourFieldDrawer("Region manager colour")]
    public Color user_region_manager_color;

    [StringFieldDrawer("Region base URL")]
    public string region_base_url;

	[StringFieldDrawer("Geoserver URL")]
	public string geoserver_url;

	[StringFieldDrawer("Geoserver username"), HideIfValue("1/geoserver_url", null)]
	public string geoserver_username;

	[StringFieldDrawer("Geoserver password"), HideIfValue("1/geoserver_url", null)]
	public string geoserver_password;

	[FloatFieldDrawer("Restriction point size")]
    public float restriction_point_size;

    [StringFieldDrawer("Windfarm API URL")]
    public string windfarm_data_api_url;
}