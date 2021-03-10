using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public abstract class AbstractFieldDrawerAttribute : Attribute
{
    protected string m_name;
    protected string m_infoText = "No info specified.";
    protected float m_nameWidth = 120f;
    protected bool m_nullable = true;
    protected int m_priority = 0;

    public AbstractFieldDrawerAttribute(string a_name)
    {
        m_name = a_name;
    }

    public virtual string Name
    {
        get { return m_name; }
    }

    public virtual bool Nullable
    {
        get { return m_nullable; }
        set { m_nullable = value; }
    }

    public virtual int Priority
    {
        get { return m_priority; }
        set { m_priority = value; }
    }

    public virtual float NameWidth
    {
        get { return m_nameWidth; }
        set { m_nameWidth = value; }
    }

    public virtual string InfoText
    {
        get { return m_infoText; }
        set { m_infoText = value; }
    }

    public abstract Type DrawerType
    {
        get;
    }

    public abstract Type FieldDataType
    {
        get;
    }
}

