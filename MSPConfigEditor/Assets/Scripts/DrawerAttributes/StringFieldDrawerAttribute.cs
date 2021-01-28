using System;

class StringFieldDrawerAttribute : InputFieldDrawerAttribute
{
    public StringFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(StringFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}
