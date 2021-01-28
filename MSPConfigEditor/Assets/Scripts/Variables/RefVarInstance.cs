using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RefVarInstance<T> : Variable<T>
{
    PathReference m_pathReference;
    T m_value;
    Action<object, DrawerEventData> m_validatedCallback;

    public override bool Valid => m_pathReference.Valid;

    public RefVarInstance(string a_reference, FieldData a_fieldData, Action<object, DrawerEventData> a_valueChangeCallback, Action a_invalidatedCallback)
    {
        m_validatedCallback = a_valueChangeCallback;
        m_pathReference = new PathReference(a_reference, a_fieldData, ValueChanged, a_invalidatedCallback, ValueChanged);
    }

    public override T GetValue()
    {
        return m_value;
    }

    public override Variable<T> GetInstance(FieldData a_fieldData, Action<object, DrawerEventData> a_valueChangeCallback, Action a_invalidatedCallback)
    {
        throw new NotImplementedException();
    }

    void ValueChanged(object a_newValue, DrawerEventData a_eventData)
    {
        m_value = (T)a_newValue;
        m_validatedCallback?.Invoke(a_newValue, a_eventData);
    }

    public override void RemoveAllReferences()
    {
        m_pathReference.RemoveAllReferences();
        m_validatedCallback = null;
    }
}
