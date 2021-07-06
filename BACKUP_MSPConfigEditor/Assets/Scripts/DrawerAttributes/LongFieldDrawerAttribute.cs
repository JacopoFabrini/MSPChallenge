using System;

public class LongFieldDrawerAttribute : InputFieldDrawerAttribute
{
    public LongFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType {
        get { return typeof(LongFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}
