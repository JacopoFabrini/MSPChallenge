using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
//using Sirenix.OdinInspector.Editor;
using TMPro;
using UnityEngine;

public class ReferenceDropdownFieldData : FieldData
{
    protected PathReference m_optionReference;

    IList m_optionData;
    public IList OptionData => m_optionData;

    object m_currentValue;
    public object CurrentValue => m_currentValue;

    int m_currentIndex = -100;
    public int CurrentIndex => m_currentIndex;

    bool m_extraValueSelected;
    public bool ExtraValueSelected => m_extraValueSelected;
    
    public override void PostCreateInitialise()
    {
        if (m_postCreateInitialised)
            return;
        base.PostCreateInitialise();
        ReferenceDropdownDrawerAttribute attribute = (ReferenceDropdownDrawerAttribute)DrawerAttribute;
        m_optionReference = new PathReference(attribute.OptionReference, this, ReferenceDataChanged, ReferenceDataInvalidated, ReferenceDataChanged);
    }

    public override void SetValue(object a_value, bool a_setDisplayValue = true, bool a_invokeCallback = true)
    {
        m_currentValue = a_value;
        base.SetValue(a_value, a_setDisplayValue, a_invokeCallback);
    }
    
    protected override void CleanUpReferences(object a_currentValue)
    {
        m_optionReference.RemoveAllReferences();
        m_optionReference = null;
        m_optionData = null;
        base.CleanUpReferences(a_currentValue);
    }

    void ReferenceDataChanged(object a_newData, DrawerEventData a_eventData)
    {
        if (a_newData == null)
        {
            ReferenceDataInvalidated();
            return;
        }
        if (!typeof(IList).IsAssignableFrom(a_newData.GetType()))
        {
            Debug.LogError("Path points towards non-list data that cannot be used for a dropdown.");
            return;
        }

        ReferenceDropdownDrawer drawer = m_drawer == null ? null : (ReferenceDropdownDrawer) m_drawer;
        m_optionData = (IList)a_newData;
        if (m_optionData != null)
        {
            bool updateHandled = false;
            if (a_eventData != null && a_eventData.PassedFromChild)
            {
                if (a_eventData.EventType == DrawerEventType.ValueChanged)
                {
                    //if (m_optionReference.Subselecting)
                    //{
                        //Only a single value changed
                        if (a_eventData.ChildIndex == m_currentIndex)
                        {
                            //Selected option changed
                            SetValue(m_optionData[m_currentIndex], false);
                            drawer?.SelectedValueChanged(m_optionData[m_currentIndex]);
                        }
                        else if (m_extraValueSelected && AreEqual(m_optionData[a_eventData.ChildIndex], m_currentValue))
                        {
                            //Value changed into our current 'extra' option, select child index instead
                            m_currentIndex = a_eventData.ChildIndex;
                            m_extraValueSelected = false;
                            drawer?.ValueChangedToExtra(m_optionData[a_eventData.ChildIndex], a_eventData.ChildIndex);
                        }
                        else
                            drawer?.NotSelectedOptionUpdated(m_optionData[a_eventData.ChildIndex], a_eventData.ChildIndex);
                    //}
                    //else
                    //{
                    //    //Only a single value changed
                    //    if (a_eventData.ChildIndex == m_currentIndex)
                    //    {
                    //        //Selected option changed
                    //        SetValue(a_eventData.ChildObject, false);
                    //        drawer?.SelectedValueChanged(a_eventData.ChildObject);
                    //    }
                    //    else if (m_extraValueSelected && AreEqual(a_eventData.ChildObject, m_currentValue))
                    //    {
                    //        //Value changed into our current 'extra' option, select child index instead
                    //        m_currentIndex = a_eventData.ChildIndex;
                    //        m_extraValueSelected = false;
                    //        drawer?.ValueChangedToExtra(a_eventData.ChildObject, a_eventData.ChildIndex);
                    //    }
                    //    else
                    //        drawer?.NotSelectedOptionUpdated(a_eventData.ChildObject, a_eventData.ChildIndex);
                    //}

                    updateHandled = true;
                }
                else if (a_eventData.EventType == DrawerEventType.Destroyed)
                {
                    //Only a single value deleted
                    if (a_eventData.ChildIndex == m_currentIndex)
                    {
                        //Selected option deleted
                        m_extraValueSelected = true;
                        m_currentIndex = m_optionData.Count;
                        drawer?.SelectedValueDeleted(a_eventData.ChildIndex);
                    }
                    else
                    {
                        //entry deleted before/after our current index
                        if (a_eventData.ChildIndex < m_currentIndex)
                            m_currentIndex--;
                        drawer?.NotSelectedValueDeleted(a_eventData.ChildIndex);
                    }

                    updateHandled = true;
                }
            }

            if (!updateHandled)
            {
                //Index or complete value change
                DoCompleteValueUpdate();
            }
        }
    }

    void DoCompleteValueUpdate()
    {
        m_currentIndex = -100;
        int i = 0;
        foreach (var option in m_optionData)
        {
            if (AreEqual(m_currentValue, option))
                m_currentIndex = i;
            i++;
        }
        if (m_currentIndex < 0)
        {
            //Our current value is not an option in the reference data, add an 'extra' option
            m_extraValueSelected = true;
            m_currentIndex = m_optionData.Count;
        }
        else
            m_extraValueSelected = false;
        if (m_drawer != null)
            ((ReferenceDropdownDrawer)m_drawer).CompleteValueChange();
    }

    public void SetSelectedIndex(int a_index)
    {
        if (a_index != m_currentIndex)
        {
            m_extraValueSelected = a_index >= m_optionData.Count;
            m_currentIndex = a_index;
            SetValue(m_optionData[a_index], false);
        }
    }

    void ReferenceDataInvalidated()
    {
        if (m_drawer != null)
        {
            ((ReferenceDropdownDrawer)m_drawer).SetInvalid();
        }
        //TODO: trigger constraint
    }

    bool AreEqual(object a, object b)
    {
        if (a == null)
            return b == null;
        return a.Equals(b);
    }
}

