using System;
using UnityEngine;

public class HideIfValueInstance : AHideIfInstance
{
    private object m_value;
    private bool m_inverse;

    public HideIfValueInstance(FieldData a_fieldData, string a_path, object a_value, bool a_inverse) : base(a_fieldData, a_path)
    {
        m_value = a_value;
        m_inverse = a_inverse;
    }

    protected override bool Evaluate(object a_target)
    {
        if (m_inverse)
            return !Equals(m_value, a_target);
        return Equals(m_value,a_target);
    }
}