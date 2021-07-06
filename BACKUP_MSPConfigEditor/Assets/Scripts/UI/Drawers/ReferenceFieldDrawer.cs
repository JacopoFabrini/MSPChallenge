using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

public abstract class ReferenceFieldDrawer<T, U> : FieldDrawer<T, U>
    where T : ReferenceFieldDrawerAttribute
    where U : FieldData
{
    protected PathReference m_nameReference;   

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();
    }

    public override void SetToFieldData(FieldData a_fieldData)
    {
        base.SetToFieldData(a_fieldData);
        if (m_attribute.GetNameFromContent)
            m_nameReference = new PathReference(m_attribute.Name, a_fieldData, UpdateName, InvalidNameReference, UpdateName);
    }

    protected virtual void UpdateName(object a_currentValue, DrawerEventData a_eventData)
    {
        if (a_currentValue != null)
            m_nameText.text = a_currentValue.ToString();
        else
            m_nameText.text = "Unnamed";
    }

    protected void InvalidNameReference()
    {
        m_nameText.text = "Invalid path";
    }

    public override void ReleaseFromFieldData()
    {
        base.ReleaseFromFieldData();
        if (m_nameReference != null)
        {
            m_nameReference.RemoveAllReferences();
            m_nameReference = null;
        }
    }
}

 