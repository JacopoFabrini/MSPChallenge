using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

class DictionaryEntryDrawer : FieldDrawer<DictionaryEntryDrawerAttribute, DictionaryEntryFieldData>
{
    [SerializeField]
    RectTransform m_keyContainer;
    public RectTransform KeyContainer => m_keyContainer;

    [SerializeField]
    RectTransform m_valueContainer;
    public RectTransform ValueContainer => m_valueContainer;

    LayoutElement m_keyLayout;
    LayoutElement m_valueLayout;
    
    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();
        //m_nameText.gameObject.SetActive(false);
        m_keyLayout = m_keyContainer.GetComponent<LayoutElement>();
        m_valueLayout = m_valueContainer.GetComponent<LayoutElement>();
    }

    public void SetKeyToValueSizeRatio(float a_keyRatio)
    {
        float clamped = Mathf.Clamp01(a_keyRatio);
        m_keyLayout.flexibleWidth = clamped;
        m_valueLayout.flexibleWidth = 1f - clamped;
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<DictionaryEntryDrawer>(this);
    }
}

