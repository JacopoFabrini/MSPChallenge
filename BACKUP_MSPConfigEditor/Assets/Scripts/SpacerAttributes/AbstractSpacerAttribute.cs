using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public abstract class AbstractSpacerAttribute : Attribute
{
    protected int m_priority = 0;

    public AbstractSpacerAttribute()
    { }

    public virtual int Priority
    {
        get { return m_priority; }
        set { m_priority = value; }
    }

    public abstract Type SpacerType
    {
        get;
    }
}

