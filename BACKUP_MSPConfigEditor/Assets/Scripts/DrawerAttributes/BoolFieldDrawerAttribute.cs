using System;

public class BoolFieldDrawerAttribute : AbstractFieldDrawerAttribute
{
    public BoolFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(BoolFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}
