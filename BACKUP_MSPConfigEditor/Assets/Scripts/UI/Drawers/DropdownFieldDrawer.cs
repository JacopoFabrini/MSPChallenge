using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownFieldDrawer : FieldDrawer<DropdownFieldDrawerAttribute, FieldData>
{
    [SerializeField]
    CustomDropdown m_dropdown;

    protected bool m_ignoreFieldChange;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();
        m_dropdown.onValueChanged.AddListener(i => { if (!m_ignoreFieldChange) DropdownValueChange(i); });
    }

    public override void SetToFieldData(FieldData a_fieldData)
    {
        base.SetToFieldData(a_fieldData);
        if (!m_fieldData.ObjectType.IsEnum)
        {
            Debug.LogError("Dropdown drawer defined for non-enum field.");
            return;
        }
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var enumValue in m_fieldData.ObjectType.GetEnumValues())
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(enumValue.ToString()));
        }
        m_dropdown.options = dropdownOptions;
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        m_ignoreFieldChange = true;
        m_dropdown.value = (int)a_value;
        m_ignoreFieldChange = false;
    }

    void DropdownValueChange(int a_newValue)
    {
        m_fieldData.SetValue(Enum.ToObject(m_fieldData.ObjectType, a_newValue), false);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<DropdownFieldDrawer>(this);
    }
}
