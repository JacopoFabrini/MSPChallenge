using System;

class InlineFieldDrawerAttribute : ReferenceFieldDrawerAttribute
{
    public InlineFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(InlineFieldDrawer); }
    }

    public override Type FieldDataType => typeof(InlineFieldData);
}