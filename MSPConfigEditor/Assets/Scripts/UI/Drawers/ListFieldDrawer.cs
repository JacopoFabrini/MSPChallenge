using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

public class ListFieldDrawer : ExpandableFieldDrawer<ListDrawerAttribute, ListFieldData>
{
    [SerializeField]
    CustomButton m_addButton;
    [SerializeField]
    RectTransform m_addButtonContainer;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_addButton.onClick.AddListener(AddButtonClicked);
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        SetExpanded(false);
        if (a_value == null)
        {
            m_nullIndicator.gameObject.SetActive(true);
            SetExpandable(false);
        }
        else
        {
            m_nullIndicator.gameObject.SetActive(false);
            SetExpandable(true);  
        }
        m_addButtonContainer.SetAsLastSibling();
    }

    void AddButtonClicked()
    {
        ((ListFieldData)m_fieldData).AddNewBaseValueChild();
        m_addButtonContainer.SetAsLastSibling();
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<ListFieldDrawer>(this);
    }

    public override void ChildIndicesUpdated()
    {
        m_addButtonContainer.SetAsLastSibling();
    }
}
