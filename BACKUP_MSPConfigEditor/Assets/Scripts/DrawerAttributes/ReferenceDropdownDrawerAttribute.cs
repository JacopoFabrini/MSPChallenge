using System;

public class ReferenceDropdownDrawerAttribute : AbstractFieldDrawerAttribute
{
    string m_optionReference;
    public ReferenceDropdownDrawerAttribute(string a_name, string a_optionReference) : base(a_name)
    {
        m_optionReference = a_optionReference;
    }

    public override Type DrawerType
    {
        get { return typeof(ReferenceDropdownDrawer); }
    }

    public override Type FieldDataType => typeof(ReferenceDropdownFieldData);


    public string OptionReference { get => m_optionReference; }
}