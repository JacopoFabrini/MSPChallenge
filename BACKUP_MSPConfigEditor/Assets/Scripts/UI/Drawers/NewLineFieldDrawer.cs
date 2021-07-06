using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Security.Permissions;
using TMPro;

public class NewLineFieldDrawer : ReferenceFieldDrawer<NewLineFieldDrawerAttribute, NewLineFieldData>
{
    [SerializeField]
    CustomToggle m_expandToggle;
    [SerializeField]
    protected RectTransform m_expandImage;
    [SerializeField]
    protected TextMeshProUGUI m_nullIndicator;

    bool m_ignoreExpandCallback;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_expandToggle.onValueChanged.AddListener(ExpandToggleChanged);
        m_expandToggle.m_onRightClick.AddListener(OnRightClicked);
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        ShowNull(a_value == null);
    }

    protected override void UpdateName(object a_currentValue, DrawerEventData a_eventData)
    {
        base.UpdateName(a_currentValue, a_eventData);
        if(m_expandToggle.isOn)
            DrawerManager.Instance.SetDrawerColumnName(m_fieldData.Depth + 1, m_nameText.text);
    }

    void ShowNull(bool a_isNull)
    {
        m_expandToggle.interactable = !a_isNull;
        m_expandImage.gameObject.SetActive(!a_isNull);
        m_nullIndicator.gameObject.SetActive(a_isNull);
        m_expandToggle.isOn = false;
    }

    void ExpandToggleChanged(bool a_expanded)
    {
        if (m_ignoreExpandCallback)
            return;
        m_fieldData?.SetExpanded(a_expanded);
    }

    public void SetExpanded(bool a_expanded)
    {
        m_ignoreExpandCallback = true;
        m_expandToggle.isOn = a_expanded;
        m_ignoreExpandCallback = false;
        if(a_expanded)
            DrawerManager.Instance.SetDrawerColumnName(m_fieldData.Depth + 1, m_nameText.text);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<NewLineFieldDrawer>(this);
    }
}