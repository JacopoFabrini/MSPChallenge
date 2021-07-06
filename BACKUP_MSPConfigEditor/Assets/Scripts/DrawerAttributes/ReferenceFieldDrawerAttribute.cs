using System;

public abstract class ReferenceFieldDrawerAttribute : AbstractFieldDrawerAttribute
{
    bool m_getNameFromContent;

    public ReferenceFieldDrawerAttribute(string a_name) : base(a_name)
    {   }

    public bool GetNameFromContent
    {
        get { return m_getNameFromContent; }
        set
        {
            m_getNameFromContent = value;
        }
    }
}