using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class ReferenceFieldData : FieldData
{
    private object m_referencedObject;
    
    public override void SetValue(object a_value, bool a_setDisplayValue = true, bool a_invokeCallback = true)
    {
        m_ignoreChildChange = true;
        ClearChildren();
        m_ignoreChildChange = false;

        m_referencedObject = a_value;

        CreateChildren();

        base.SetValue(a_value, a_setDisplayValue, a_invokeCallback);
    }

    public override object GetDataObject()
    {
        return m_referencedObject;
    }

    protected override void CleanUpReferences(object a_currentValue)
    {
        ClearChildren();
        base.CleanUpReferences(a_currentValue);
    }

    public override void Select()
    {
        base.Select();
        SetExpanded(true);
    }

    protected abstract void ClearChildren();
    protected abstract void CreateChildren();
}
