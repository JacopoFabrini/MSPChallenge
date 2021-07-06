using System;

public class HideIfValueAttribute : AHideIfAttribute
{
    private object m_value;
    private bool m_inverse;

    public HideIfValueAttribute(string a_path, object a_value, bool a_inverse = false) : base(a_path)
    {
        m_value = a_value;
        m_inverse = a_inverse;
    }

    public override AHideIfInstance GetInstance(FieldData a_fieldData)
    {
        return new HideIfValueInstance(a_fieldData, m_path, m_value, m_inverse);
    }

}

