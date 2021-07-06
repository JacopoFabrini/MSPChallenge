using System;

public class DropdownFieldDrawerAttribute : AbstractFieldDrawerAttribute
{
    public DropdownFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type DrawerType
    {
        get { return typeof(DropdownFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);
}