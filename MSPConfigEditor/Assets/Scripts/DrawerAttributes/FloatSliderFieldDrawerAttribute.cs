using System;

public class FloatSliderFieldDrawerAttribute : InputFieldDrawerAttribute
{
    float m_min;
    float m_max;

    public FloatSliderFieldDrawerAttribute(string a_name, float a_min, float a_max) : base(a_name)
    {
        m_min = a_min;
        m_max = a_max;
    }

    public override Type DrawerType
    {
        get { return typeof(FloatSliderFieldDrawer); }
    }

    public override Type FieldDataType => typeof(FieldData);

    public float Min { get => m_min; }
    public float Max { get => m_max; }
}