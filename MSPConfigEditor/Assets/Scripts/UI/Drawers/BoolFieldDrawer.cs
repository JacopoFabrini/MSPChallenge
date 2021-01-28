using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoolFieldDrawer : FieldDrawer<BoolFieldDrawerAttribute, FieldData>
{
    [SerializeField]
    protected CustomToggle m_toggle;
    [SerializeField]
    protected GameObject m_nullIndicator;

    private bool m_ignoreFieldChange;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_toggle.onValueChanged.AddListener(ToggleChanged);
        m_toggle.m_onRightClick.AddListener(OnRightClicked);
    }

    void ToggleChanged(bool a_newValue)
    {
        if (m_ignoreFieldChange)
            return;

        m_fieldData?.SetValue(a_newValue);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<BoolFieldDrawer>(this);
        m_ignoreFieldChange = false;
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        m_ignoreFieldChange = true;
        if (a_value == null)
        {
            m_nullIndicator.SetActive(true);
            m_toggle.gameObject.SetActive(false);
        }
        else
        {
            m_nullIndicator.SetActive(false);
            m_toggle.gameObject.SetActive(true);
            m_toggle.isOn = (bool)a_value;
        }
        m_ignoreFieldChange = false;
    }
}