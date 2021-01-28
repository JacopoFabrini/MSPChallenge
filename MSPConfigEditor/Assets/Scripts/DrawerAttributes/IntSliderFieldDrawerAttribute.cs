using System;

public class IntSliderFieldDrawerAttribute : InputFieldDrawerAttribute
{
    int m_min;
    int m_max;

    public IntSliderFieldDrawerAttribute(string a_name, int a_min, int a_max) : base(a_name)
    {
        m_min = a_min;
        m_max = a_max;
    }

    public override Type DrawerType
    {
        get { return typeof(IntSliderFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);

    public int Min { get => m_min; }
    public int Max { get => m_max; }
}