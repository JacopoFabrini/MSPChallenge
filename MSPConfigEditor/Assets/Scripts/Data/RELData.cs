using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class RELData
{
	[NewLineFieldDrawer("Input")]
	public RELInput input;

	[NewLineFieldDrawer("Output")]
	public RELOutput output;
}

[Serializable]
public class RELInput
{
	[ListDrawer("Points", Priority = 0),
	InlineFieldDrawer("0/point_id", GetNameFromContent = true, Priority = 1)]
	public List<RELPoint> points;

	[ListDrawer("Links", Priority = 0),
		InlineFieldDrawer("0/link_id", GetNameFromContent = true, Priority = 1)]
	public List<RELLink> links;

	[ListDrawer("Traffic", Priority = 0),
		InlineFieldDrawer("0/link_id", GetNameFromContent = true, Priority = 1)]
	public List<RELTraffic> traffic;

	[ListDrawer("Restriction Data", Priority = 0),
		InlineFieldDrawer("0/geometry_id", GetNameFromContent = true, Priority = 1)]
	public List<RELRestrictionData> restriction_data;
}

[Serializable]
public class RELPoint
{
	public int point_id;
	public double lat;
	public double lon;
}

[Serializable]
public class RELLink
{
	public int link_id;
	public int point_id_start;
	public int point_id_end;
	public float link_width;
}

[Serializable]
public class RELTraffic
{
	public int link_id;
	public int ship_type;
	public int intensity;
}

[Serializable]
public class RELRestrictionData
{
	public int geometry_id;
	public int point_id_start;
	public int geometry_type;
}

[Serializable]
public class RELOutput
{

}

[Serializable]
public class RELOutputArea
{

}

[Serializable]
public class RELOutputData
{

}

