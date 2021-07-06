using System;

public class IntFieldDrawerAttribute : InputFieldDrawerAttribute
{
    public IntFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(IntFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}
