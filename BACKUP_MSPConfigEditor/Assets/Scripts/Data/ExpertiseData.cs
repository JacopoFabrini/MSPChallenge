using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


[Serializable]
public class ExpertiseData
{
    [StringFieldDrawer("Expertise Name")]
    public string name;

    [ListDrawer("Visible Layers", Priority = 0), ReferenceDropdownDrawer(null, "-1/meta.layer_name", Priority = 1)]
    public List<string> visible_layers;

    [ListDrawer("Selected Layers", Priority = 0), ReferenceDropdownDrawer(null, "-1/meta.layer_name", Priority = 1)]
    public List<string> selected_layers;
}