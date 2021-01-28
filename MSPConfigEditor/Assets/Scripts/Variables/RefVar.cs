using System;
using System.Collections.Generic;


public class RefVar<T> : Variable<T>
{
    string m_reference;

    public override bool Valid => true;

    public RefVar(string a_reference)
    {
        m_reference = a_reference;
    }

    public override T GetValue()
    {
        throw new NotImplementedException();
    }

    public override Variable<T> GetInstance(FieldData a_fieldData, Action<object, DrawerEventData> a_valueChangeCallback, Action a_invalidatedCallback)
    {
        return new RefVarInstance<T>(m_reference, a_fieldData, a_valueChangeCallback, a_invalidatedCallback);
    }

    public override void RemoveAllReferences()
    {}
}

