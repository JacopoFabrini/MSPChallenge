using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Linq;

public class DictionaryFieldDrawer : ExpandableFieldDrawer<DictionaryFieldDrawerAttribute, DictionaryFieldData>
{
    [SerializeField]
    DictionaryEntryDrawer m_entryDrawerPrefab;
    [SerializeField]
    CustomButton m_addButton;
    [SerializeField]
    RectTransform m_addButtonContainer;
    
    float m_keyTovalueSizeRatio = 0.5f;

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
        m_fieldData.AddNewBaseValueChild();
        m_addButtonContainer.SetAsLastSibling();
    }

    public void SetKeyToValueSizeRatio(float a_keyRatio)
    {
        m_fieldData.SetKeyToValueSizeRatio(a_keyRatio);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<DictionaryFieldDrawer>(this);
    }

    public override void ChildIndicesUpdated()
    {
        m_addButtonContainer.SetAsLastSibling();
    }
}

