using System;

public class ColourFieldDrawerAttribute : AbstractFieldDrawerAttribute
{
    public ColourFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(ColourFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}