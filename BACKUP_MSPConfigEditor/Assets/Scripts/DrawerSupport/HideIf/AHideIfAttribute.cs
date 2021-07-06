using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public abstract class AHideIfAttribute : Attribute
{
    protected string m_path;

    protected AHideIfAttribute(string a_path)
    {
        m_path = a_path;
    }

    public abstract AHideIfInstance GetInstance(FieldData a_fieldData);
}

