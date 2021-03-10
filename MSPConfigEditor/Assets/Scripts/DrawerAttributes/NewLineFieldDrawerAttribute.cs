using System;

public class NewLineFieldDrawerAttribute : ReferenceFieldDrawerAttribute
{
    public NewLineFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(NewLineFieldDrawer); }
    }

    public override Type FieldDataType => typeof(NewLineFieldData);
}