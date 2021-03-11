using System;
using System.Collections.Generic;

public class ConstVar<T> : Variable<T>
{
    T m_value;

    public override bool Valid => true;

    public ConstVar(T a_value)
    {
        m_value = a_value;
    }

    public override T GetValue()
    {
        return m_value;
    }

    public override Variable<T> GetInstance(FieldData a_fieldData, Action<object, DrawerEventData> a_valueChangeCallback, Action a_invalidatedCallback)
    {
        return this;
    }

    public override void RemoveAllReferences()
    { }
}

