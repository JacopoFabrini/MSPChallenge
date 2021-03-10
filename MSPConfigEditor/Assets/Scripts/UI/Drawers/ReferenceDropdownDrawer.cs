using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ReferenceDropdownDrawer : FieldDrawer<ReferenceDropdownDrawerAttribute, ReferenceDropdownFieldData>
{
    [SerializeField]
    CustomDropdown m_dropdown;
    [SerializeField]
    GameObject m_invalidDataDisplay;

    protected bool m_ignoreFieldChange;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_dropdown.onValueChanged.AddListener(i => { if (!m_ignoreFieldChange) DropdownValueChange(i); });
    }
    
    public override void UpdateDisplayedValue(object a_value)
    {
        CompleteValueChange();
    }

    public void SelectedValueChanged(object a_newValue)
    {
        string displayText = GetDisplayText(a_newValue);
        m_dropdown.options[m_fieldData.CurrentIndex] = new TMP_Dropdown.OptionData(displayText);
        m_dropdown.captionText.text = displayText;
    }

    public void SelectedValueDeleted(int a_index)
    {
        m_ignoreFieldChange = true;
        m_dropdown.options.RemoveAt(a_index);
        m_dropdown.options.Add(new TMP_Dropdown.OptionData(GetDisplayText(m_fieldData.CurrentValue)));
        m_dropdown.value = m_fieldData.CurrentIndex;
        m_ignoreFieldChange = false;
    }

    public void NotSelectedValueDeleted(int a_index)
    {
        m_ignoreFieldChange = true;
        m_dropdown.options.RemoveAt(a_index);
        m_dropdown.value = m_fieldData.CurrentIndex;
        m_ignoreFieldChange = false;
    }

    public void ValueChangedToExtra(object a_newValue, int a_index)
    {
        m_dropdown.options[a_index] = new TMP_Dropdown.OptionData(GetDisplayText(a_newValue));
        m_ignoreFieldChange = true;
        m_dropdown.value = a_index;
        m_dropdown.options.RemoveAt(m_dropdown.options.Count - 1);
        m_ignoreFieldChange = false;
    }

    public void NotSelectedOptionUpdated(object a_newValue, int a_index)
    {
        m_dropdown.options[a_index] = new TMP_Dropdown.OptionData(GetDisplayText(a_newValue));
    }

    public void CompleteValueChange()
    {
        m_invalidDataDisplay.SetActive(false);
        m_dropdown.gameObject.SetActive(true);
        
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var option in m_fieldData.OptionData)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(GetDisplayText(option)));
        }
        m_ignoreFieldChange = true;
        if (m_fieldData.ExtraValueSelected)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(GetDisplayText(m_fieldData.CurrentValue)));
        }
        m_dropdown.options = dropdownOptions;
        m_dropdown.value = m_fieldData.CurrentIndex;
        m_ignoreFieldChange = false;
    }

    void DropdownValueChange(int a_newValue)
    {
        if (m_fieldData.ExtraValueSelected && a_newValue < m_fieldData.OptionData.Count)
        {
            //Extra option deselected
            m_dropdown.options.RemoveAt(m_dropdown.options.Count - 1);
        }

        m_fieldData.SetSelectedIndex(a_newValue);
    }

    string GetDisplayText(object a_value)
    {
        return a_value == null ? "Null" : a_value.ToString();
    }

    public void SetInvalid()
    {
        m_invalidDataDisplay.SetActive(true);
        m_dropdown.gameObject.SetActive(false);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<ReferenceDropdownDrawer>(this);
    }
}
