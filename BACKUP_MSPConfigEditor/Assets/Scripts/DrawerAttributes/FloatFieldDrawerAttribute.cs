using System;

class FloatFieldDrawerAttribute : InputFieldDrawerAttribute
{
    public FloatFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(FloatFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}
