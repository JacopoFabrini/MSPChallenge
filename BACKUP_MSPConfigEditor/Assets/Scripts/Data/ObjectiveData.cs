using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ObjectiveData
{
    [IntFieldDrawer("Country ID")]
    public int country_id;
    [StringFieldDrawer("Title")]
    public string title;
    [StringFieldDrawer("Description")]
    public string description;
    [IntFieldDrawer("Deadline")]
    public int deadline;
}