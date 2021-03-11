using System;

public class DoubleFieldDrawerAttribute : InputFieldDrawerAttribute
{
    public DoubleFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(DoubleFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}
