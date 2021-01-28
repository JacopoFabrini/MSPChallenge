using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class DictionaryEntryFieldData : FieldData
{
    Type m_keyType;
    Type m_valueType;
    AbstractFieldDrawerAttribute[] m_keyAttribute;
    AbstractFieldDrawerAttribute[] m_valueAttributes;
    FieldData m_keyFieldData;
    FieldData m_valueFieldData;
    Dictionary<int, List<IConstraintDefinition>> m_subConstraints;
    
    public override void PostCreateInitialise()
    {
        if (m_postCreateInitialised)
            return;
        base.PostCreateInitialise();
        m_keyFieldData.PostCreateInitialise();
        m_valueFieldData.PostCreateInitialise();
    }

    protected override void Initialise(int a_depth, FieldData a_parent, object a_target, AbstractFieldDrawerAttribute[] a_attributes, Type a_contentType, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, AHideIfAttribute a_hideIfAttribute)
    {
        if (a_attributes.Length < 3)
        {
            Debug.LogError("No subdrawers specified for either keys or values.");
            return;
        }

        m_subConstraints = a_constraints;
        m_keyType = a_contentType.GetGenericArguments()[0];
        m_valueType = a_contentType.GetGenericArguments()[1];
        m_keyAttribute = new AbstractFieldDrawerAttribute[1] { a_attributes[1] };
        m_valueAttributes = new AbstractFieldDrawerAttribute[a_attributes.Length - 2];
        for (int i = 2; i < a_attributes.Length; i++)
            m_valueAttributes[i - 2] = a_attributes[i];

        base.Initialise(a_depth, a_parent, a_target, a_attributes, a_contentType, a_constraints, a_spacerAttributes, a_hideIfAttribute);
    }

    public override void SetValue(object a_value, bool a_setDisplayValue = true, bool a_invokeCallback = true)
    {
        if (a_setDisplayValue)
        {
            DictionaryData entryValue = (DictionaryData)a_value;
            if (m_keyFieldData == null)
            {
                m_keyFieldData  = DrawerManager.CreateObjectFieldData(m_depth, this, entryValue.Key, m_keyAttribute, m_keyType, m_subConstraints, null, m_postCreateInitialised);
                m_keyFieldData.Index = 0;
            }
            else
                m_keyFieldData.SetValue(entryValue.Key, true, false);

            if (m_valueFieldData == null)
            {
                m_valueFieldData = DrawerManager.CreateObjectFieldData(m_depth, this, entryValue.Value, m_valueAttributes, m_valueType, m_subConstraints, null, m_postCreateInitialised);
                m_valueFieldData.Index = 1;
            }
            else
                m_valueFieldData.SetValue(entryValue.Value, true, false);
        }

        base.SetValue(a_value, a_setDisplayValue, a_invokeCallback);
    }

    public override void CreateDrawer(Transform a_drawerParent = null)
    {
        base.CreateDrawer(a_drawerParent);
        DictionaryEntryDrawer drawer = (DictionaryEntryDrawer) m_drawer;
        m_keyFieldData.CreateDrawer(drawer.KeyContainer);
        m_valueFieldData.CreateDrawer(drawer.ValueContainer);
    }

    public override void ReleaseDrawer()
    {
        base.ReleaseDrawer();
        m_keyFieldData.ReleaseDrawer();
        m_valueFieldData.ReleaseDrawer();
    }


    protected override void ChildChanged(DrawerEventData a_eventData)
    {
        if (m_isBeingDestroyed || m_ignoreChildChange)
            return;
        if (a_eventData.PassedFromChild)
            base.ChildChanged(a_eventData);
        else
        {
            //Dont pass it on as a childchanged event, but as our own
            PassOnEventFromChild(new DrawerEventData(DrawerEventType.ValueChanged, GetDataObject(), Index, a_eventData.TargetIndex, a_eventData.TargetObject));
        }
    }

    public override object GetValueOfChild(int a_childIndex)
    {
        DictionaryData data = (DictionaryData)m_parent.GetValueOfChild(Index);
        if (a_childIndex == 0)
            return data.Key;
        return data.Value;
    }

    protected override void CleanUpReferences(object a_currentValue)
    {
        DictionaryData data = (DictionaryData)a_currentValue;
        if (m_keyFieldData != null)
            m_keyFieldData.DestroyFieldData(data.Key);
        if (m_valueFieldData != null)
            m_valueFieldData.DestroyFieldData(data.Value);
        base.CleanUpReferences(a_currentValue);
    }

    public void SetKeyToValueSizeRatio(float a_keyRatio)
    {
        if (m_drawer != null)
        {
            ((DictionaryEntryDrawer) m_drawer).SetKeyToValueSizeRatio(a_keyRatio);
        }
    }
}

